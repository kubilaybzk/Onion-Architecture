using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.DTOs.Payment
{
    public class PaymentRequest
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Cvc { get; set; }
        public bool Use3D { get; set; }
        public string Currency { get; set; }
        public string OrderId { get; set; }
        public string BasketId { get; set; }
        public string ReturnUrl { get; set; }
        public Order OrderInformation  { get; set; }
    }
}
