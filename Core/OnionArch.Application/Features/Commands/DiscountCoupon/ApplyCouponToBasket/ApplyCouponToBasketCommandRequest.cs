using MediatR;

namespace OnionArch.Application.Features.Commands.DiscountCoupon.ApplyCouponToBasket
{
    public class ApplyCouponToBasketCommandRequest : IRequest<ApplyCouponToBasketCommandResponse>
    {
        public string CouponCode { get; set; }
    }
} 