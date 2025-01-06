using MediatR;

namespace OnionArch.Application.Features.Commands.DiscountCoupon.RemoveCouponFromBasket
{
    public class RemoveCouponFromBasketCommandRequest : IRequest<RemoveCouponFromBasketCommandResponse>
    {
        public string BasketId { get; set; }
    }
} 