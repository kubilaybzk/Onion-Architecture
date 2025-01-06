using OnionArch.Application.GlobalResponse;

namespace OnionArch.Application.Features.Commands.DiscountCoupon.UpdateDiscountCoupon
{
    public class UpdateDiscountCouponCommandResponse : GlobalResponseResult
    {
        public Guid CouponId { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
    }
} 