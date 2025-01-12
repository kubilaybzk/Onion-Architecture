using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.BasketServices;
using OnionArch.Application.Abstractions.OrderCrud;
using OnionArch.Application.Abstractions.OrderServices;
using OnionArch.Domain.Entities;
using OnionArch.Domain.Entities.Identity;
using OnionArch.Domain.Enums;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.ServicesConcreates
{
    public class OrderService : IOrderService
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IBasketService _basketService;
        private readonly OnionArchDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(
            IOrderReadRepository orderReadRepository,
            IOrderWriteRepository orderWriteRepository,
            IBasketService basketService,
            OnionArchDBContext context,
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _orderReadRepository = orderReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _basketService = basketService;
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Order> CreateOrderFromBasketAsync(string username, string shippingAddress, string billingAddress)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Sepet bilgilerini al
                var basket = await _basketService.CurrentUserBasket();
                var basketItems = await _basketService.GetBasketItemsAsync();

                if (basketItems == null || !basketItems.Any())
                    throw new Exception("Sepetinizde ürün bulunmamaktadır.");
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                    throw new Exception("Kullanıcı bulunamadı");
                // Yeni sipariş oluştur
                var order = new Order
                {
                    OrderNo = Guid.NewGuid().ToString("N")[..10].ToUpper(),
                    UserId = user.Id,
                    Status = OrderStatus.Created,
                    ShippingAddress = shippingAddress,
                    BillingAddress = billingAddress,
                    DiscountCouponId = basket.DiscountCouponId,
                    DiscountedAmount = basket.DiscountedAmount,
                    Basket = basket,
                    OrderItems = basketItems.Select(bi => new OrderItem
                    {
                        ProductId = bi.ProductId,
                        Quantity = bi.Quantity,
                        UnitPrice = bi.Product.LastPrice,
                        TotalPrice = bi.Product.LastPrice * bi.Quantity
                    }).ToList()
                };

                order.TotalAmount = order.OrderItems.Sum(oi => oi.TotalPrice);

                await _orderWriteRepository.AddAsync(order);
                await _orderWriteRepository.SaveAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            return await _orderReadRepository.Table
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .Include(o => o.DiscountCoupon)
                .FirstOrDefaultAsync(o => o.ID == Guid.Parse(id));
        }

        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            return await _orderReadRepository.Table
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreateTime)
                .ToListAsync();
        }

        

        public async Task<decimal> CalculateOrderTotalAsync(string orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order == null)
                throw new Exception("Sipariş bulunamadı");

            return order.TotalAmount;
        }

        public async Task<bool> UpdateOrderStatusAsync(string orderId, OrderStatus status)
        {
            var order = await _orderReadRepository.GetByIdAsync(orderId);
            if (order == null)
                return false;

            order.Status = status;
            _orderWriteRepository.Update(order);
            await _orderWriteRepository.SaveAsync();
            return true;
        }
    }
}