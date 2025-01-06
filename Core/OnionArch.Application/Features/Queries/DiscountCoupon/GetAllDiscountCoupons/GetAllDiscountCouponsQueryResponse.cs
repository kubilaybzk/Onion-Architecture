using OnionArch.Application.GlobalResponse;

namespace OnionArch.Application.Features.Queries.DiscountCoupon.GetAllDiscountCoupons
{
    public class GetAllDiscountCouponsQueryResponse : GlobalResponseResult
    {
        public int TotalCount { get; set; }
        public IEnumerable<DiscountCouponDto> Coupons { get; set; }
    }

    public class DiscountCouponDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumCartAmount { get; set; }
        public bool IsPercentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool IsPersonal { get; set; }
        public string UserId { get; set; }
        public int MaxUsageCount { get; set; }
        public int UsedCount { get; set; }
        public bool IsActive { get; set; }
    }
} 