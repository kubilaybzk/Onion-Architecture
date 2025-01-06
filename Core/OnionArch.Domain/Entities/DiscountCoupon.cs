using OnionArch.Domain.Entities.Common;
using OnionArch.Domain.Entities.Identity;

namespace OnionArch.Domain.Entities
{
    public class DiscountCoupon : BaseEntity
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
        public AppUser? User { get; set; }
        public int MaxUsageCount { get; set; }
        public int UsedCount { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Basket> UsedInBaskets { get; set; }
    }
} 