using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.DiscountServices;
using OnionArch.Application.Repositories.DiscountCouponCrud;
using OnionArch.Domain.Entities;
using OnionArch.Domain.Entities.Identity;
using OnionArch.Persistance.Contexts;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.BasketServices;

namespace OnionArch.Persistance.ServicesConcreates
{
    public class DiscountCouponService : IDiscountCouponService
    {
        private readonly IDiscountCouponReadRepository _discountCouponReadRepository;
        private readonly IDiscountCouponWriteRepository _discountCouponWriteRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly OnionArchDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBasketService _basketService;

        public DiscountCouponService(
            IDiscountCouponReadRepository discountCouponReadRepository,
            IDiscountCouponWriteRepository discountCouponWriteRepository,
            UserManager<AppUser> userManager,
            OnionArchDBContext context,
            IHttpContextAccessor httpContextAccessor,
            IBasketService basketService)
        {
            _discountCouponReadRepository = discountCouponReadRepository;
            _discountCouponWriteRepository = discountCouponWriteRepository;
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _basketService = basketService;
        }

        public async Task<bool> ApplyCouponToBasketAsync(string code)
        {
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new Exception("Kullanıcı bulunamadı");

            var basket = await _basketService.CurrentUserBasket();
            if (basket == null)
                throw new Exception("Aktif sepet bulunamadı");

            var coupon = await ValidateAndGetCouponAsync(code, basket.UserId);
            if (coupon == null)
                return false;

            var basketItems = await _basketService.GetBasketItemsAsync();
            decimal cartTotal = basketItems.Sum(bi => bi.Product.LastPrice * bi.Quantity);
            
            if (cartTotal < coupon.MinimumCartAmount)
                throw new Exception($"Minimum sepet tutarı {coupon.MinimumCartAmount:C2} olmalıdır");

            decimal discountAmount = await CalculateDiscountAsync(coupon, cartTotal);

            basket.DiscountCouponId = coupon.ID;
            basket.DiscountedAmount = discountAmount;

            await _context.SaveChangesAsync();
            await IncrementCouponUsageAsync(coupon.ID);

            return true;
        }

        public async Task<decimal> CalculateDiscountAsync(DiscountCoupon coupon, decimal cartTotal)
        {
            if (coupon.IsPercentage)
            {
                decimal percentageRate = Math.Floor(coupon.DiscountAmount / 10) * 10;
                decimal discountAmount = cartTotal * (coupon.DiscountAmount / 100);
                return cartTotal - discountAmount;
            }
            else
            {
                return cartTotal - coupon.DiscountAmount;
            }
        }

        public async Task<bool> IncrementCouponUsageAsync(Guid couponId)
        {
            var coupon = await _discountCouponReadRepository.GetByIdAsync(couponId.ToString());
            if (coupon == null)
                return false;

            coupon.UsedCount++;
            _discountCouponWriteRepository.Update(coupon);
            await _discountCouponWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DecrementCouponUsageAsync(Guid couponId)
        {
            var coupon = await _discountCouponReadRepository.GetByIdAsync(couponId.ToString());
            if (coupon == null)
                return false;

            if (!coupon.IsActive || coupon.UsedCount > 0)
            {
                coupon.UsedCount--;
                _discountCouponWriteRepository.Update(coupon);
                await _discountCouponWriteRepository.SaveAsync();
            }

            return true;
        }

        public async Task<bool> RemoveCouponFromBasketAsync()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                    throw new Exception("Kullanıcı bulunamadı");

                var basket = await _basketService.CurrentUserBasket();
                if (basket == null)
                    throw new Exception("Aktif sepet bulunamadı");

                if (basket == null || !basket.DiscountCouponId.HasValue)
                    return false;

                await DecrementCouponUsageAsync(basket.DiscountCouponId.Value);

                basket.DiscountCouponId = null;
                basket.DiscountedAmount = null;
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<DiscountCoupon> ValidateAndGetCouponAsync(string code, string userId)
        {
            var coupon = await _discountCouponReadRepository.GetWhere(c => c.Code == code)
                .FirstOrDefaultAsync();

            if (coupon == null || !coupon.IsActive)
                throw new Exception("Geçersiz kupon kodu");

            if (DateTime.UtcNow < coupon.ValidFrom || DateTime.UtcNow > coupon.ValidUntil)
                throw new Exception("Kupon süresi geçmiş veya henüz başlamamış");

            if (coupon.UsedCount >= coupon.MaxUsageCount)
                throw new Exception("Kupon kullanım limiti dolmuş");

            if (coupon.IsPersonal && coupon.UserId != userId)
                throw new Exception("Bu kupon size özel değil");

            return coupon;
        }
    }
} 