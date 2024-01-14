
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnionArch.Application.View_Models.BasketItem;

namespace OnionArch.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryResponse
    {

        public List<VM_Result_BasketList> BasketItems { get; set; }
        public float TotalProductPrice { get; set; }
        public float TotalDiscount { get; set; }
        public float CargoPrice { get; set; }

        public float TotalPrice { get; set; }



    }
}
