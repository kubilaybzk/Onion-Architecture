
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnionArch.Application.GlobalResponse;
using OnionArch.Application.View_Models.BasketItem;

namespace OnionArch.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryResponse : GlobalResponseResult
    {

        public List<VM_Result_BasketList> BasketItems { get; set; }
        public float TotalBasketOriginalPrice { get; set; }
        public float TotalBasketDiscount { get; set; }
        public float CargoPrice { get; set; }
        public float TotolBasketLastPrice { get; set; }

        public Guid? DiscountCouponId { get; set; }
        public decimal DiscountCouponValue { get; set; }
        public decimal? DiscountedCuponAmount { get; set; }
        public bool? IsCuponIsPercentage { get; set; }
        public string? DiscountCouponName { get; set; }

    }
}
