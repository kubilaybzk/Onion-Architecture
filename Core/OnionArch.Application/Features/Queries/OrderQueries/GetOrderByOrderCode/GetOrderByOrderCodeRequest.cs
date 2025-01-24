using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.OrderQueries.GetOrderByOrderCode
{
    public class GetOrderByOrderCodeRequest : IRequest<GetOrderByOrderCodeResponse>
    {
        public string OrderCode { get; set; }
    }
}
