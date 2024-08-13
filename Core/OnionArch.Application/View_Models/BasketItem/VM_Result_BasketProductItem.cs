using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.View_Models.BasketItem
{
    public class VM_Result_BasketProductItem
    {
        public string ProductId { get; set; }
        public string ProductImg { get; set; }
        public string ProductName { get; set; }
        public decimal ProductOriginalPrice { get; set; }
        public decimal ProductLastPrice { get; set; }
        public string ProductCurrency { get; set; }
        public int ProductQuantity { get; set; }
        public DateTime ProductAddedTime { get; set; }
        public string ProductSlug { get; set; }
    }
}
