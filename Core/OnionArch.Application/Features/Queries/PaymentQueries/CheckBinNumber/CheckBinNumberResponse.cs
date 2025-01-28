using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.PaymentQueries.CheckBinNumber
{
    public class CheckBinNumberResponse:GlobalResponseResult
    {
        public string ProviderResponse { get; set; }
        public int? Commerical { get; set; }
        public string? BankName { get; set; }
        public string? BinNumber { get; set; }
        public string? CardFamily { get; set; }
        public string? CardAssociation { get; set; }
        public string? CardType { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string? status { get; set; }
        public string ErrorGroup { get; set; }
    }
}
