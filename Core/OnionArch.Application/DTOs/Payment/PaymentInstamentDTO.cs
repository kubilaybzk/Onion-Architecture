using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.DTOs.Payment
{
    public class PaymentInstamentDTO
    {
        public string? ErrorCode { get; set; }
        public string? ErrorGroup { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Status { get; set; }
        public List<InstallmentDetail>? installmentDetails { get; set; } //Taksit oranları için
    }

    public class InstallmentDetail
    {
        public string binNumber { get; set; }
        public double price { get; set; }
        public string cardType { get; set; }
        public string cardAssociation { get; set; }
        public string cardFamilyName { get; set; }
        public int force3ds { get; set; }
        public int bankCode { get; set; }
        public string bankName { get; set; }
        public int forceCvc { get; set; }
        public int commercial { get; set; }
        public int dccEnabled { get; set; }
        public List<InstallmentPrice>? installmentPrices { get; set; }
    }

    public class InstallmentPrice
    {
        public string installmentPrice { get; set; }
        public string totalPrice { get; set; }
        public int installmentNumber { get; set; }
}

}