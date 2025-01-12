using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.Payment.Complete3DPayment
{
    public class Complete3DResponse : GlobalResponseResult
    {
        public bool PaymentSuccess { get; set; }
        public string TransactionId { get; set; }
    }
}
