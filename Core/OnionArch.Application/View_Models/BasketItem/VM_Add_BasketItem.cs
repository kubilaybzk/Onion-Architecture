using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.View_Models.BasketItem
{
    public class VM_Add_BasketItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; } = 1; //default olarak 1 olsun eğer veri gelmiyor ise.
    }
}
