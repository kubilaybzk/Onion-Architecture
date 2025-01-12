using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.DTOs.Payment
{
    public class PaymentHistoryDTO
    {
        public string TransactionId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string PaymentProvider { get; set; }
        public string OrderNo { get; set; }
        public string ErrorMessage { get; set; }
    }
}
