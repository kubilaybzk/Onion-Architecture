using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.OrderQueries.GetUserOrders
{
    public class GetUserOrdersResponse : GlobalResponseResult
    {
        public List<UserOrderDTO> Orders { get; set; }
    }
    public class UserOrderDTO
    {
        public string OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime CreateDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? DiscountedAmount { get; set; }
        public List<OrderItemDTO> Items { get; set; }
    }

    public class OrderItemDTO
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
