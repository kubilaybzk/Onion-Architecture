using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.Payment.CreatePayment
{
    public class CreatePaymentResponse : GlobalResponseResult
    {
        public string TransactionId { get; set; }
        public string RedirectUrl { get; set; }  // 3D için
        public bool RequiresRedirect { get; set; }
        public string OrderId { get; set; }
        public bool  isCreated { get; set; }
    }
}
