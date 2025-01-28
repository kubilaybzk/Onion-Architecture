using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.PaymentQueries.CheckBinNumber
{
    public class CheckBinNumberRequest :IRequest<CheckBinNumberResponse>
    {
        public string cardNumber { get; set; }
    }
}
