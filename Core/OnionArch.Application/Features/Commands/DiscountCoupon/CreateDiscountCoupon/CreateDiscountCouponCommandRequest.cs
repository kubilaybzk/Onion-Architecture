using MediatR;

namespace OnionArch.Application.Features.Commands.DiscountCoupon.CreateDiscountCoupon
{
    public class CreateDiscountCouponCommandRequest : IRequest<CreateDiscountCouponCommandResponse>
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumCartAmount { get; set; }
        public bool IsPercentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool IsPersonal { get; set; }
        public string? UserId { get; set; }
        public int MaxUsageCount { get; set; }
    }
} 