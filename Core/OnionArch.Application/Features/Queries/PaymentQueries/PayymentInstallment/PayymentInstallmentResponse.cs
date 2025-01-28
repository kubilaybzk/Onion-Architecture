using OnionArch.Application.DTOs.Payment;
using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.PaymentQueries.PayymentInstallment
{
    public class PayymentInstallmentResponse:GlobalResponseResult
    {
        
            public string? ErrorCode { get; set; }
            public string? ErrorGroup { get; set; }
            public string? ErrorMessage { get; set; }
            public string? Status { get; set; }
            public List<InstallmentDetail>? installmentDetails { get; set; } //Taksit oranları için
       
    }
}
