using OnionArch.Application.DTOs.Payment;
using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.PaymentQueries
{
    public class GetPaymentHistoryResponse : GlobalResponseResult
    {
        public List<PaymentHistoryDTO> Payments { get; set; }
    }
}
