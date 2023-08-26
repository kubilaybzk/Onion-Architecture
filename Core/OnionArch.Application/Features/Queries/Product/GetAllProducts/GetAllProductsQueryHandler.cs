
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Features.Queries.Product.Product.GetAllProducts;

namespace OnionArch.Application.Features.Queries.Product.GetAllProducts
{
    //burada MediatR'a sana gelecek request bu şekilde bu request için işlem yap ve responce olarak bunu döndür diyoruz.
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
    {

        //Burada dependency injection'dan yararlanıyoruz.
        private readonly IProductReadRepository _productReadRepository;

        public GetAllProductsQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }


        public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                //Operasyonumuzu burada tanımlayacağız.
                var productQuery = _productReadRepository.GetAll(false);

                int totalProductCount = await productQuery.CountAsync();

                int totalPageSize = (int)Math.Ceiling((double)totalProductCount / request.Size);
                var pagedProductQuery = productQuery.Skip(request.Size * request.Page).Take(request.Size);
                int pageSize = await pagedProductQuery.CountAsync();
                var productResult = await pagedProductQuery
                .Select(p => new
                {
                    p.Name,
                    p.Price,
                    p.Stock,
                    p.ID,
                    p.ProductImageFiles,
                    p.CreateTime,
                    p.UpdateTime
                })
                .ToListAsync();
                // JSON dönüşümü için liste haline getiriyoruz


                bool hasNextPage = request.Page < totalPageSize - 1;
                bool hasPrevPage = request.Page > 0;

                return new GetAllProductsQueryResponse()
                {
                    TotalCount = totalProductCount,
                    TotalPageSize = totalPageSize,
                    CurrentPage = request.Page,
                    HasNext = hasNextPage,
                    HasPrev = hasPrevPage,
                    PageSize = pageSize,
                    Products = productResult,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Ürün Gönderildi"
                };
            }
            catch(Exception ex)
            {
                return new GetAllProductsQueryResponse()
                {

                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = ex.Message.ToString()
                };
            }

        }


    }
}

