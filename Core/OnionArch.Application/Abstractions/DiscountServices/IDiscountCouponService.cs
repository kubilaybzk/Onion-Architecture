using OnionArch.Domain.Entities;

namespace OnionArch.Application.Abstractions.DiscountServices
{
    public interface IDiscountCouponService
    {
        Task<DiscountCoupon> ValidateAndGetCouponAsync(string code, string userId);
        Task<decimal> CalculateDiscountAsync(DiscountCoupon coupon, decimal cartTotal);
        Task<bool> ApplyCouponToBasketAsync(string code);
        Task<bool> RemoveCouponFromBasketAsync();
        Task<bool> IncrementCouponUsageAsync(Guid couponId);
    }
} 