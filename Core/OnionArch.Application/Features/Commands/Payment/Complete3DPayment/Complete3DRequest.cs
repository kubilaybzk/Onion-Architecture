using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.Payment.Complete3DPayment
{
    public class Complete3DRequest : IRequest<Complete3DResponse>
    {
        public string PaymentId { get; set; }
        public string ThreeDResponse { get; set; }
    }
}
