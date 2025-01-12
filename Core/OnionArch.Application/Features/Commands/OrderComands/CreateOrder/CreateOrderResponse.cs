using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.OrderComands.CreateOrder
{
    public class CreateOrderResponse : GlobalResponseResult
    {
        public string OrderId { get; set; }
        public string OrderNo { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? DiscountedAmount { get; set; }
    }
}
