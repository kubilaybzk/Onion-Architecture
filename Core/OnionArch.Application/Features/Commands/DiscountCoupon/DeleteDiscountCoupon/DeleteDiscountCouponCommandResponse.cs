using OnionArch.Application.GlobalResponse;

namespace OnionArch.Application.Features.Commands.DiscountCoupon.DeleteDiscountCoupon
{
    public class DeleteDiscountCouponCommandResponse : GlobalResponseResult
    {
        public Guid DeletedCouponId { get; set; }
    }
} 