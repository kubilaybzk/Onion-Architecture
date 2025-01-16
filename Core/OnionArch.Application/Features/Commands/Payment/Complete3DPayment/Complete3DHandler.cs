using MediatR;
using OnionArch.Application.Abstractions.PaymentServices;
using OnionArch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.Payment.Complete3DPayment
{
    public class Complete3DHandler : IRequestHandler<Complete3DRequest, Complete3DResponse>
    {
        private readonly IPaymentService _paymentService;

        public Complete3DHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<Complete3DResponse> Handle(Complete3DRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var transaction = await _paymentService.CompleteThreeDPaymentAsync(request.PaymentId, request.ConversationData);
               
                var response = new Complete3DResponse
                {
                    PaymentSuccess = transaction.Status == PaymentStatus.Success,
                    TransactionId = transaction.ID.ToString(),
                    Message = transaction.Status == PaymentStatus.Success ? "Ödeme başarılı" : "Ödeme başarısız",
                    StatusCode = transaction.Status == PaymentStatus.Success ?
                        System.Net.HttpStatusCode.OK :
                        System.Net.HttpStatusCode.BadRequest,
                    HassError = false,
                    
                };
                if (transaction.Status == PaymentStatus.Failed)
                {
                    response.ErrorMessage = transaction.ErrorMessage;
                }
                return response;
            }
            catch (Exception ex)
            {
                return new Complete3DResponse
                {
                    Message = "3D doğrulama işlemi başarısız",
                    ErrorMessage = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    HassError = true
                };
            }
        }
    }
}


