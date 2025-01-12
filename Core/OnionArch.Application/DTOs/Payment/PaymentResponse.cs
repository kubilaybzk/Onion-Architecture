using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.DTOs.Payment
{
    public class PaymentResponse
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string RedirectUrl { get; set; }
        public string ProviderResponse { get; set; }
    }
}
