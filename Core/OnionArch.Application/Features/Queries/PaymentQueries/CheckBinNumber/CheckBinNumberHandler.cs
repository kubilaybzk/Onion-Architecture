using MediatR;
using OnionArch.Application.Abstractions.PaymentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.PaymentQueries.CheckBinNumber
{
    public class CheckBinNumberHandler : IRequestHandler<CheckBinNumberRequest, CheckBinNumberResponse>
    {
        private readonly IPaymentService _paymentService;

        public CheckBinNumberHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<CheckBinNumberResponse> Handle(CheckBinNumberRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var binNumber = await  _paymentService.GetPaymentBinNumberAsync(request.cardNumber);
                if (binNumber.Status == "success")
                {
                    return new CheckBinNumberResponse()
                    {
                        Message = "Taksit sayısı başarıyle çekildi",
                        HassError = false,
                        BankName = binNumber.BankName,
                        BinNumber = binNumber.BinNumber,
                        CardAssociation = binNumber.CardAssociation,
                        CardFamily = binNumber.CardFamily,
                        CardType = binNumber.CardType,
                        Commerical = binNumber.Commerical,
                        status = binNumber.Status,
                        StatusCode = System.Net.HttpStatusCode.OK,
                        StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),

                    };
                }
                else
                {
                    return new CheckBinNumberResponse()
                    {
                        Message = "Taksit sayısı çelilemedi",
                        HassError = true,
                        status = binNumber.Status,
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        StatusCodeString = System.Net.HttpStatusCode.NotFound.ToString(),

                    };
                }
            }
            catch (Exception ex)
            {
                return new CheckBinNumberResponse()
                {
                    status="failure",
                    ErrorMessage = ex.Message,
                    HassError = false,
                    Message = "Bin Number Checked function ServerError",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                };
            }
        }
    }
}
