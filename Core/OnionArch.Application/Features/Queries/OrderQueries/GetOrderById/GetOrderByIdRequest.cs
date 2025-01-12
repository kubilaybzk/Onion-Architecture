using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.OrderQueries.GetOrderById
{
    public class GetOrderByIdRequest : IRequest<GetOrderByIdResponse>
    {
        public string OrderId { get; set; }
    }
}
