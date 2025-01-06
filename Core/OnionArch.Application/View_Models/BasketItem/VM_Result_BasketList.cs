using OnionArch.Application.GlobalResponse;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.View_Models.BasketItem
{
    public class VM_Result_BasketList
    {
        public string BasketItemId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductId { get; set; }
        public string ProductImg { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string BrandSlug { get; set; }
        public decimal ProductOriginalPrice { get; set; }
        public decimal ProductLastPrice { get; set; }
        public string ProductCurrency { get; set; }
        public DateTime ProductAddedTime { get; set; }
        public string ProductSlug { get; set; }
        public decimal DiscountRate { get; set; } // İndirim oranı
        public decimal DiscountPrice { get; set; } // İndirimli fiyatı


        //public VM_Result_BasketProductItem Products { get; set; }
    }
}
