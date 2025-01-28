using MediatR;
using OnionArch.Application.Abstractions.PaymentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;

namespace OnionArch.Application.Features.Queries.PaymentQueries.PayymentInstallment
{
    public class PayymentInstallmentHandler : IRequestHandler<PayymentInstallmentRequest, PayymentInstallmentResponse>
    {
        private readonly IPaymentService _paymentService;

        public PayymentInstallmentHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<PayymentInstallmentResponse> Handle(PayymentInstallmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paymentService.GetPaymentInstallment(request.CardNumbeer, request.PaidPrice);
                if (response.Status == "success")
                {
                    return new PayymentInstallmentResponse
                    {
                        ErrorCode = response.ErrorCode,
                        ErrorGroup = response.ErrorGroup,
                        ErrorMessage = response.ErrorMessage,
                        Status = response.Status,
                        installmentDetails = response.installmentDetails,
                        HassError=false,
                        StatusCode=System.Net.HttpStatusCode.OK,
                        StatusCodeString= System.Net.HttpStatusCode.OK.ToString()
                    };
                }
                else
                {
                    return new PayymentInstallmentResponse
                    {
                        ErrorCode = response.ErrorCode,
                        ErrorGroup = response.ErrorGroup,
                        ErrorMessage = response.ErrorMessage,
                        Status = response.Status,
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        StatusCodeString = System.Net.HttpStatusCode.NotFound.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                return new PayymentInstallmentResponse
                {
                    ErrorMessage = ex.Message,
                    HassError = true,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString()
                };
            }
            
        }
    }
}
