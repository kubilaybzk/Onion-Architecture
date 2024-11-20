using MediatR;
using OnionArch.Application.Abstractions.BasketServices;
using OnionArch.Application.Features.Commands.Basket.AddItemToBasket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.Basket.RemoveBasketItem
{
    public class RemoveBasketItemCommandHandler : IRequestHandler<RemoveBasketItemCommandRequest, RemoveBasketItemCommandResponse>
    {
        readonly IBasketService _basketService;

        public RemoveBasketItemCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<RemoveBasketItemCommandResponse> Handle(RemoveBasketItemCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Boolean result = await _basketService.RemoveBasketItemAsync(request.BasketItemId);

                if (result)
                {
                    return new RemoveBasketItemCommandResponse()
                    {
                        ErrorMessage = null,
                        HassError = false,
                        isDeleted = true,
                        Message = "Ürün başarıyla sepetten silindi",
                        StatusCode = System.Net.HttpStatusCode.OK,
                        StatusCodeString = System.Net.HttpStatusCode.OK.ToString()
                    };
                }
                else
                {
                    return new RemoveBasketItemCommandResponse()
                    {
                        ErrorMessage = "",
                        HassError = true,
                        isDeleted = false,
                        Message = "Ürün sepetten silinirken hata alındı",
                        StatusCode = System.Net.HttpStatusCode.NotAcceptable,
                        StatusCodeString = System.Net.HttpStatusCode.NotAcceptable.ToString()
                    };

                }
            }
            catch(Exception ex)
            {
                return new RemoveBasketItemCommandResponse()
                {
                    ErrorMessage = ex.Message,
                    HassError = true,
                    isDeleted = false,
                    Message = "Ürün sepetten silinirken hata alındı",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString()
                };
            }

        }
    }
}
