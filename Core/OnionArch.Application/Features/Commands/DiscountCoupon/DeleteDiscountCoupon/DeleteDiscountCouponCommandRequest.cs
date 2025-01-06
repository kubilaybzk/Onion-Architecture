using MediatR;

namespace OnionArch.Application.Features.Commands.DiscountCoupon.DeleteDiscountCoupon
{
    public class DeleteDiscountCouponCommandRequest : IRequest<DeleteDiscountCouponCommandResponse>
    {
        public Guid Id { get; set; }
    }
} 