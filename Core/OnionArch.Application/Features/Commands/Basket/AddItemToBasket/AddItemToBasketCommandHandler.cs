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

        async Task<AddItemToBasketCommandResponse> IRequestHandler<AddItemToBasketCommandRequest, AddItemToBasketCommandResponse>.Handle(AddItemToBasketCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketService.AddBasketItemToBasketAsync(new()
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity
            });

            return new();
        }
    }
}
