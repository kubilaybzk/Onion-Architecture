using OnionArch.Application.Features.Queries.OrderQueries.GetUserOrders;
using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.OrderQueries.GetOrderById
{
    public class GetOrderByIdResponse : GlobalResponseResult
    {
        public UserOrderDTO Order { get; set; }
    }
}
