using MediatR;
using OnionArch.Application.Abstractions.OrderServices;
using OnionArch.Application.Abstractions.PaymentServices;
using OnionArch.Application.DTOs.Payment;
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
                var order = await _orderService.GetOrderByIdAsync(request.OrderId);
                if (order == null)
                    throw new Exception("Sipariş bulunamadı");

                var paymentRequest = new PaymentRequest
                {
                    OrderId = request.OrderId,
                    CardHolderName = request.CardHolderName,
                    CardNumber = request.CardNumber,
                    ExpirationMonth = request.ExpireMonth,
                    ExpirationYear = request.ExpireYear,
                    Cvc = request.Cvc,
                    Use3D = request.Use3D,
                    Amount = order.TotalAmount,
                    Currency = "TRY" // Configden alınabilir
                };

                var transaction = await _paymentService.CreatePaymentTransactionAsync(request.OrderId, paymentRequest);

                var processedTransaction = await _paymentService.ProcessPaymentAsync(transaction.ID.ToString());


                if (processedTransaction.PaymentStatus == "success")
                {
                    return new CreatePaymentResponse
                    {
                        TransactionId = processedTransaction.ID.ToString(),
                        RedirectUrl = processedTransaction.ProviderResponse, // 3D URL'i response içinden alınmalı
                        RequiresRedirect = request.Use3D,
                        Message = "Ödeme işlemi başarılı",
                        StatusCode = System.Net.HttpStatusCode.OK,
                        HassError = false,
                        isCreated = true,
                        OrderId = processedTransaction.OrderId.ToString()

                    };
                }
                else
                {
                    return new CreatePaymentResponse
                    {
                        TransactionId = processedTransaction.ID.ToString(),
                        RedirectUrl = processedTransaction.ProviderResponse, // 3D URL'i response içinden alınmalı
                        RequiresRedirect = request.Use3D,
                        Message = "Ödeme işlemi başarısız",
                        ErrorMessage=processedTransaction.ErrorMessage,
                        StatusCode = System.Net.HttpStatusCode.OK,
                        HassError = false,
                        isCreated = false,
                        OrderId = processedTransaction.OrderId.ToString()

                    };
                }
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

