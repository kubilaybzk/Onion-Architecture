using MediatR;
using OnionArch.Application.Abstractions.OrderServices;
using OnionArch.Application.Abstractions.PaymentServices;
using OnionArch.Application.DTOs.Payment;
using OnionArch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.Payment.CreatePayment
{
    public class CreatePaymentHandler : IRequestHandler<CreatePaymentRequest, CreatePaymentResponse>
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public CreatePaymentHandler(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }

        public async Task<CreatePaymentResponse> Handle(CreatePaymentRequest request, CancellationToken cancellationToken)
        {
            try
            {

                var paymentRequest = new PaymentRequest
                {
                    OrderId = request.OrderId,
                    CardHolderName = request.CardHolderName,
                    CardNumber = request.CardNumber,
                    ExpirationMonth = request.ExpireMonth,
                    ExpirationYear = request.ExpireYear,
                    Cvc = request.Cvc,
                    Use3D = request.Use3D,
                    Currency = "TRY",
                };

                var transaction = await _paymentService.CreatePaymentTransactionAsync(request.OrderId, paymentRequest);
                
                // 3D ödeme için
               
                    if (transaction.Status == PaymentStatus.Failed)
                    {
                        return new CreatePaymentResponse
                        {
                            TransactionId = transaction.ID.ToString(),
                            HtmlContent = transaction.ProviderResponse, // 3D için HTML içeriği
                            RequiresRedirect = false,
                            Message = "Ödeme işlemi başlatılamadı",
                            ErrorMessage=transaction.ErrorMessage,
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            HassError = true,
                            isCreated = false,
                            OrderId = transaction.OrderId.ToString()
                        };
                    }
                        
                        var response = new CreatePaymentResponse
                        {
                            TransactionId = transaction.ID.ToString(),
                            RequiresRedirect = true,
                            Message = "Ödeme işlemi  başlatıldı",
                            StatusCode = System.Net.HttpStatusCode.OK,
                            HassError = false,
                            isCreated = true,
                            OrderId = transaction.OrderId.ToString()
                        };
                        if (transaction.IsThreeD)
                        {
                            response.HtmlContent = transaction.ProviderResponse; // 3D için HTML içeriği
                        }
                return response;
                 
               
            }
            catch (Exception ex)
            {
                return new CreatePaymentResponse
                {
                    Message = "Ödeme işlemi başlatılırken hata oluştu",
                    ErrorMessage = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    HassError = true,
                    isCreated = false
                };
            }
        }
    }
}

