using MediatR;
using OnionArch.Application.Abstractions.BasketServices;
using OnionArch.Application.Features.Queries.Basket.GetBasketItems;
using OnionArch.Application.View_Models.BasketItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Basket.GetOnlyBasketsPayment
{
    public class GetOnlyBasketsPaymentHandler : IRequestHandler<GetOnlyBasketsPaymentRequest, GetOnlyBasketsPaymentResponce>
    {
        public readonly IBasketService _basketService;

        public GetOnlyBasketsPaymentHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<GetOnlyBasketsPaymentResponce> Handle(GetOnlyBasketsPaymentRequest request, CancellationToken cancellationToken)
        {
            var basketItems = await _basketService.GetBasketItemsAsync();

            //Burada ufak bir bug var o düzenlenecek bir sonraki aşamada. 
            //Todo

            // Toplam fiyat hesaplaması


            var result = basketItems
                .Select(ba => new VM_Result_BasketList
                {
                    BasketItemId = ba.ID.ToString(),
                    Quantity = ba.Quantity,
                    Price=ba.Product.Price,
                    
                }).ToList();

            float totalProductPrice = result.Sum(ba => ba.Price * ba.Quantity);
            float totalDiscount;
            float totalCargoPrice;
            float totalPrice;

            // Toplam indirim hesaplaması (%10)
            if (totalProductPrice > 0)
            {
                totalDiscount = totalProductPrice * 0.10f;
                totalCargoPrice = totalProductPrice > 1000 ? 0 : 20.00f;
                totalPrice = totalProductPrice - totalDiscount + totalCargoPrice;
            }
            else
            {
                totalDiscount = 0;
                totalCargoPrice = 0;

                totalPrice = 0;
            }

            return new GetOnlyBasketsPaymentResponce()
            {
                TotalProductPrice = totalProductPrice,
                TotalDiscount = totalDiscount,
                CargoPrice = totalCargoPrice,
                TotalPrice = totalPrice,
            };

        }
    }
}
