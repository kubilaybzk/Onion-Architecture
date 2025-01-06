using OnionArch.Application.GlobalResponse;

namespace OnionArch.Application.Features.Commands.DiscountCoupon.RemoveCouponFromBasket
{
    public class RemoveCouponFromBasketCommandResponse : GlobalResponseResult
    {
        public decimal? RemovedDiscountAmount { get; set; }
    }
} 