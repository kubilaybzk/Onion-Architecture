using MediatR;
using OnionArch.Application.Abstractions.BasketServices;
using OnionArch.Application.View_Models.BasketItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQueryRequest, GetBasketItemsQueryResponse>
    {

        readonly IBasketService _basketService;

        public GetBasketItemsQueryHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<GetBasketItemsQueryResponse> Handle(GetBasketItemsQueryRequest request, CancellationToken cancellationToken)
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
                    Products = new VM_Result_BasketProductItem()
                    {
                         ProductSlug=ba.Product.MaterializedProductPathBySlug,
                         ProductName=ba.Product.Name,
                         ProductQuantity=ba.Quantity,
                         ProductLastPrice = ba.Product.LastPrice,
                         ProductId=ba.Product.ID.ToString(),
                         ProductCurrency=ba.Product.Currency,
                         ProductImg = ba.Product.ProductImageFiles.Where(p => p.Showcase == true).Select(p => p.Path).ToList()[0].ToString(),
                         ProductOriginalPrice=ba.Product.UnitPrice,
                         ProductAddedTime=ba.CreateTime
                    }
                }).ToList();

            var totalProductPrice = result.Sum(ba => ba.Products.ProductLastPrice * ba.Quantity);

            float totalDiscount;
            float totalCargoPrice;
            float totalPrice;

            // Toplam indirim hesaplaması (%10)
            if (totalProductPrice > 0)
            {
                 totalDiscount =/* totalProductPrice * 0.10f;*/ 100;
                 totalCargoPrice = totalProductPrice > 1000 ? 0 : 20.00f;
                 totalPrice =/* totalProductPrice - totalDiscount*/ 100 + totalCargoPrice;
            }
            else
            {
                 totalDiscount =0;
                 totalCargoPrice = 0;

                 totalPrice =0;
            }

            return new GetBasketItemsQueryResponse()
            {
                BasketItems = result,
                TotalProductPrice = /*totalProductPrice*/ 100,
                TotalDiscount = totalDiscount,
                CargoPrice= totalCargoPrice,
                TotalPrice= totalPrice,
            };
        }
    }
}
