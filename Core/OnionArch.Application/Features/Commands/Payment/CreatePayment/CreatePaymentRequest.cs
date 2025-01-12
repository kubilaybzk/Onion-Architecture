using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.Payment.CreatePayment
{
    public class CreatePaymentRequest : IRequest<CreatePaymentResponse>
    {
        public string OrderId { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }
        public string Cvc { get; set; }
        public bool Use3D { get; set; }
    }
}
