using System;
using OnionArch.Domain.Entities.Common;
using OnionArch.Domain.Entities.Identity;
using OnionArch.Domain.Enums;

namespace OnionArch.Domain.Entities
{
	public class Order: BaseEntity
    {
        public string OrderNo { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? DiscountedAmount { get; set; }
        public Guid? DiscountCouponId { get; set; }
        public DiscountCoupon DiscountCoupon { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public Basket Basket { get; set; }
    }
}

