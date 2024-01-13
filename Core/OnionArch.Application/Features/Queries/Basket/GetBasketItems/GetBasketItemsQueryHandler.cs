using MediatR;
using OnionArch.Application.Abstractions.BasketServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQueryRequest, List<GetBasketItemsQueryResponse>>
    {

        readonly IBasketService _basketService;

        public GetBasketItemsQueryHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<List<GetBasketItemsQueryResponse>> Handle(GetBasketItemsQueryRequest request, CancellationToken cancellationToken)
        {
            var basketItems = await _basketService.GetBasketItemsAsync();

            //Burada ufak bir bug var o düzenlenecek bir sonraki aşamada. 
            //Todo

            var result = basketItems
                .Select(ba => new GetBasketItemsQueryResponse
                {
                    BasketItemId = ba.ID.ToString(),
                    Quantity = ba.Quantity,
                    Product = new()
                    {
                        ProductImageFiles = ba.Product.ProductImageFiles.Select(bas => new OnionArch.Domain.Entities.ProductImageFile()
                        {
                            FileName = bas.FileName,
                            Showcase = bas.Showcase,
                            ID = bas.ID,
                            Path = bas.Path,

                        }).ToList(),
                        Stock = ba.Quantity,
                        Name = ba.Product.Name,
                        Price = ba.Product.Price,
                    }
                    }).ToList();



            return result;
        }
    }
}
