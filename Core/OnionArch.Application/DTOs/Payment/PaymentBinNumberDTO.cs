using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.DTOs.Payment
{
    public class PaymentBinNumberDTO
    {
        public string ProviderResponse { get; set; }
        public int? Commerical { get; set; }
        public string? BankName { get; set; }
        public string? BinNumber { get; set; }
        public string? CardFamily { get; set; }
        public string? CardAssociation { get; set; }
        public string? CardType { get; set; }
        public string? Status { get; set; }
    }
}
