using MediatR;
using OnionArch.Application.Abstractions.BasketServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.Basket.AddMultipleItemToBasket
{
    public class AddMultipleItemToBasketHandler : IRequestHandler<AddMultipleItemToBasketRequest, AddMultipleItemToBasketResponse>
    {
        readonly IBasketService _basketService;

        public AddMultipleItemToBasketHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }
        public async Task<AddMultipleItemToBasketResponse> Handle(AddMultipleItemToBasketRequest request, CancellationToken cancellationToken)
        {
            try
            {
                bool Added = await _basketService.AddMultipleBasketItemsToBasketAsync(request.BasketItems);
                if(Added)
                {
                    return new AddMultipleItemToBasketResponse()
                    {
                        ErrorMessage = "",
                        HassError = false,
                        isAdded = true,
                        Message = "Ürünler başarıyla eklendi",
                        StatusCode = System.Net.HttpStatusCode.OK,
                        StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                    };
                }
                else
                {
                    return new AddMultipleItemToBasketResponse()
                    {
                        ErrorMessage = "",
                        HassError = false,
                        isAdded = false,
                        Message = "Ürünler eklenemedi",
                        StatusCode = System.Net.HttpStatusCode.OK,
                        StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                    };
                }
            }
            catch (Exception ex)
            {
                return new AddMultipleItemToBasketResponse()
                {
                    ErrorMessage = ex.Message,
                    HassError = true,
                    isAdded = false,
                    Message = "Backend servisinde bir hata alındı",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString(),
                };
            }
        }
    }
}
