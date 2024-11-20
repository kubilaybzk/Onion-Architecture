using MediatR;
using OnionArch.Application.Abstractions.BasketServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.Basket.AddItemToBasket
{
    public class AddItemToBasketCommandHandler : IRequestHandler<AddItemToBasketCommandRequest, AddItemToBasketCommandResponse>
    {
        readonly IBasketService _basketService;

        public AddItemToBasketCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<AddItemToBasketCommandResponse> Handle(AddItemToBasketCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Boolean result = await _basketService.AddBasketItemToBasketAsync(new()
                {
                    ProductId = request.BasketItemId,
                    Quantity = request.Quantity
                });

                if (result)
                {
                    return new AddItemToBasketCommandResponse()
                    {
                        ErrorMessage = null,
                        HassError = false,
                        IsAdded = true,
                        Message = "Ürün başarıyla sepete eklendi",
                        StatusCode = System.Net.HttpStatusCode.OK,
                        StatusCodeString = System.Net.HttpStatusCode.OK.ToString()
                    };
                }
                return new AddItemToBasketCommandResponse()
                {
                    ErrorMessage = "Ürün eklenemedi",
                    HassError = true,
                    IsAdded = false,
                    Message = "Ürün başarıyla sepete ekelenemedi",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                };

            }
            catch (Exception e)
            {
                return new AddItemToBasketCommandResponse()
                {
                    ErrorMessage = e.Message,
                    HassError = true,
                    IsAdded = false,
                    Message = "Ürün başarıyla sepete ekelenemedi",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString()
                };
            }
        }
    }
}
