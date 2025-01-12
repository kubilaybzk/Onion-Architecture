using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.OrderComands.CreateOrder
{
    public class CreateOrderRequest : IRequest<CreateOrderResponse>
    {
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
    }
}
