using MediatR;

namespace OnionArch.Application.Features.Commands.DiscountCoupon.UpdateDiscountCoupon
{
    public class UpdateDiscountCouponCommandRequest : IRequest<UpdateDiscountCouponCommandResponse>
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumCartAmount { get; set; }
        public DateTime ValidUntil { get; set; }
        public int MaxUsageCount { get; set; }
        public bool IsActive { get; set; }
    }
} 