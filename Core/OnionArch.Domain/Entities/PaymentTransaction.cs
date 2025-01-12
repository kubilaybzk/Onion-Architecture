using OnionArch.Domain.Entities.Common;
using OnionArch.Domain.Entities.Identity;
using OnionArch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Entities
{
    public class PaymentTransaction : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string PaymentProvider { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentStatus Status { get; set; }
        public bool IsThreeD { get; set; }
        public string CardNumber { get; set; }  // Maskelenmiş
        public string CardHolder { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ProviderResponse { get; set; }
    }
}
