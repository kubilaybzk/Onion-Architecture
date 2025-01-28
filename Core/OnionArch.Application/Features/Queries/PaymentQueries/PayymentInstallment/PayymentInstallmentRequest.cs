using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.PaymentQueries.PayymentInstallment
{
    public class PayymentInstallmentRequest:IRequest<PayymentInstallmentResponse>
    {
        public string CardNumbeer { get; set; }
        public double PaidPrice { get; set; }
    }
}
