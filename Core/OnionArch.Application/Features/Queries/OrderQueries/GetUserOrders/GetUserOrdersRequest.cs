using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.OrderQueries.GetUserOrders
{
    public class GetUserOrdersRequest : IRequest<GetUserOrdersResponse>
    {
    }
}
