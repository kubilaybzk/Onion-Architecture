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
                .Select(ba => new VM_Result_BasketList()
                {
                    BasketItemId = ba.ID.ToString(),
                    Quantity = ba.Quantity,
                    ProductSlug = ba.Product.MaterializedProductPathBySlug,
                    ProductName = ba.Product.Name,
                    ProductLastPrice = ba.Product.LastPrice,
                    ProductId = ba.Product.ID.ToString(),
                    ProductCurrency = ba.Product.Currency,
                    ProductImg = ba.Product.ProductImageFiles.Where(p => p.Showcase == true).Select(p => p.Path).ToList()[0].ToString(),
                    ProductOriginalPrice = ba.Product.UnitPrice,
                    ProductAddedTime = ba.CreateTime,
                    DiscountPrice = ba.Product.DiscountPrice,
                    DiscountRate = ba.Product.DiscountRate,
                    Price = ba.Product.UnitPrice,
                    BrandName = ba.Product.Brand.BrandName,
                    BrandSlug = ba.Product.Brand.BrandSlug,
                }).ToList();

            var   totalDiscountedProducts = result.Sum(ba => (float)(ba.ProductLastPrice * ba.Quantity)); //Ürün fiyatı * toplam adet
            float totalOriginalPrice = result.Sum(ba => (float)(ba.ProductOriginalPrice * ba.Quantity));
            float totalDiscount;
            float totalCargoPrice;
            float totalPrice;

            if (totalDiscountedProducts > 0)
            {
                // Toplam indirim (%10)
                totalDiscount = totalOriginalPrice - totalDiscountedProducts; //Ürünlere yapılan toplam indirim

                // Kargo ücreti hesaplama (1000 TL üzeri ücretsiz)
                totalCargoPrice = totalDiscountedProducts > 1000 ? 0 : 20.00f;

                // Toplam fiyat (Ürünler - İndirim + Kargo)
                totalPrice = totalOriginalPrice - totalDiscount + totalCargoPrice; //Tüm sepetin toplam fiyatı indirimli
            }
            else
            {
                totalDiscount = 0;
                totalCargoPrice = 0;
                totalPrice = 0;
            }



            return new GetBasketItemsQueryResponse()
            {
                BasketItems = result,
                TotalBasketDiscount = totalDiscount,
                TotolBasketLastPrice = totalPrice,
                TotalBasketOriginalPrice = totalOriginalPrice,
                CargoPrice = totalCargoPrice,
                DiscountCouponValue = basketItems.Select(b => b.Basket?.DiscountCoupon?.DiscountAmount)
                                .FirstOrDefault(code => code != null) ?? 0m,
                DiscountedCuponAmount = basketItems.Select(b => b.Basket.DiscountedAmount).FirstOrDefault(),
                IsCuponIsPercentage = basketItems.Select(b => b.Basket?.DiscountCoupon?.IsPercentage).FirstOrDefault(code => code != null),
                ErrorMessage = null,
                HassError = false,
                Message = "Başarıyla getirildi",
                StatusCode = System.Net.HttpStatusCode.OK,
                StatusCodeString = System.Net.HttpStatusCode.OK.ToString()

            };
        }
    }
}
