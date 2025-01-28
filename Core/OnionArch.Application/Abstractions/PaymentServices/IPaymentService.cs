using OnionArch.Application.DTOs.Payment;
using OnionArch.Domain.Entities;
using OnionArch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Abstractions.PaymentServices
{
    public interface IPaymentService
    {
        Task<PaymentTransaction> CreatePaymentTransactionAsync(string orderId, PaymentRequest request);
        Task<PaymentTransaction> ProcessPaymentAsync(string transactionId);
        Task<PaymentTransaction> CompleteThreeDPaymentAsync(string paymentId, string threeDResponse);
        Task<PaymentTransaction> GetTransactionByIdAsync(string transactionId);
        Task<List<PaymentTransaction>> GetOrderTransactionsAsync(string orderId);
        Task<List<PaymentTransaction>> GetUserTransactionsAsync(string userId);
        Task UpdateTransactionStatusAsync(string transactionId, PaymentStatus status, string? errorMessage = null);

        Task <PaymentBinNumberDTO> GetPaymentBinNumberAsync(string cardNumber);
        Task<PaymentInstamentDTO> GetPaymentInstallment(string cardNumber, double paidPrice);
    }
}
