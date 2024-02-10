using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Basket.GetOnlyBasketsPayment
{
    public class GetOnlyBasketsPaymentResponce
    {
        public float TotalProductPrice { get; set; }
        public float TotalDiscount { get; set; }
        public float CargoPrice { get; set; }

        public float TotalPrice { get; set; }
    }
}
