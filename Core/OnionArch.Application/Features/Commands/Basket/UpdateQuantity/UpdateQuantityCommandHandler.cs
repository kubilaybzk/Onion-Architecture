using MediatR;
using OnionArch.Application.Abstractions.BasketServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.Basket.UpdateQuantity
{
    public class UpdateQuantityCommandHandler : IRequestHandler<UpdateQuantityCommandRequest, UpdateQuantityCommandResponse>
    {
        readonly IBasketService _basketService;

        public UpdateQuantityCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        async Task<UpdateQuantityCommandResponse> IRequestHandler<UpdateQuantityCommandRequest, UpdateQuantityCommandResponse>.Handle(UpdateQuantityCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketService.UpdateBasketItemAsync(new()
            {
                BasketItemId = request.BasketItemId,
                Quantity = request.Quantity
            });

            return new();
        }
    }
}
