using OnionArch.Application.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Abstractions.PaymentServices
{
    public interface IPaymentProvider
    {
        string ProviderName { get; }
        Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);
        Task<PaymentResponse> ProcessThreeDPaymentAsync(string paymentId,string conversationId, string threeDResponse);
        Task<PaymentResponse> CancelPaymentAsync(string transactionId);
        Task<PaymentResponse> RefundPaymentAsync(string transactionId, decimal amount);
        Task<PaymentResponse> CheckBinNumber(string binNumber);
        Task<PaymentInstamentDTO> GetBasketInstament(string cardNumber, string paidPrice);
    }
}
