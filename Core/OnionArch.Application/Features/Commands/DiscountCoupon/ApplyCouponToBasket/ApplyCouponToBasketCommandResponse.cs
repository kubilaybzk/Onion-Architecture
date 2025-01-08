using OnionArch.Application.GlobalResponse;

namespace OnionArch.Application.Features.Commands.DiscountCoupon.ApplyCouponToBasket
{
    public class ApplyCouponToBasketCommandResponse : GlobalResponseResult
    {
        public decimal DiscountedAmount { get; set; }
        public decimal FinalPrice { get; set; }

        public string AppliedCuponCode { get; set; }
    }
} 