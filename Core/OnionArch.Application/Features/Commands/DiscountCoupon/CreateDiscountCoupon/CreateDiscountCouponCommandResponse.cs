using OnionArch.Application.GlobalResponse;

namespace OnionArch.Application.Features.Commands.DiscountCoupon.CreateDiscountCoupon
{
    public class CreateDiscountCouponCommandResponse : GlobalResponseResult
    {
        public Guid CouponId { get; set; }
        public string Code { get; set; }
    }
} 