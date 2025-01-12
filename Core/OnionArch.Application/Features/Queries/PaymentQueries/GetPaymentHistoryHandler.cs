using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.PaymentServices;
using OnionArch.Application.DTOs.Payment;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.PaymentQueries
{
    public class GetPaymentHistoryHandler : IRequestHandler<GetPaymentHistoryRequest, GetPaymentHistoryResponse>
    {
        private readonly IPaymentService _paymentService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetPaymentHistoryHandler(
            IPaymentService paymentService,
            IHttpContextAccessor httpContextAccessor)
        {
            _paymentService = paymentService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetPaymentHistoryResponse> Handle(GetPaymentHistoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                    throw new Exception("Kullanıcı bulunamadı");

                List<PaymentTransaction> transactions;
                if (!string.IsNullOrEmpty(request.OrderId))
                {
                    transactions = await _paymentService.GetOrderTransactionsAsync(request.OrderId);
                }
                else
                {
                    transactions = await _paymentService.GetUserTransactionsAsync(username);
                }

                var paymentHistory = transactions.Select(t => new PaymentHistoryDTO
                {
                    TransactionId = t.TransactionId,
                    Date = t.CreateTime,
                    Amount = t.Amount,
                    Status = t.Status.ToString(),
                    PaymentProvider = t.PaymentProvider,
                    OrderNo = t.Order?.OrderNo,
                    ErrorMessage = t.ErrorMessage
                }).ToList();

                return new GetPaymentHistoryResponse
                {
                    Message = "Ödeme geçmişi başarıyla listendi",
                    Payments = paymentHistory,
                    StatusCode = System.Net.HttpStatusCode.OK,
                    HassError=false,
                };
            }
            catch (Exception ex)
            {
                return new GetPaymentHistoryResponse
                {
                    Message = "Ödeme geçmişi alınırken hata oluştu",
                    ErrorMessage = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    HassError = true
                };
            }
        }
    }
}
