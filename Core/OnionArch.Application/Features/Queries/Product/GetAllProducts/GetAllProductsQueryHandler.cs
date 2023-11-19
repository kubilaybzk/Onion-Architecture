
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Features.Queries.Product.Product.GetAllProducts;

namespace OnionArch.Application.Features.Queries.Product.GetAllProducts
{
    //burada MediatR'a sana gelecek request bu şekilde bu request için işlem yap ve responce olarak bunu döndür diyoruz.
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
    {

        //Burada dependency injection'dan yararlanıyoruz.
        private readonly IProductReadRepository _productReadRepository;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        public GetAllProductsQueryHandler(IProductReadRepository productReadRepository, ILogger<GetAllProductsQueryHandler> logger)
        {
            _productReadRepository = productReadRepository;
            _logger = logger;
        }


        public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                //Operasyonumuzu burada tanımlayacağız.
                var productQuery = _productReadRepository.GetAll(false);

                /*
                 Şimdi buruda birden fazla yöntem kullanabiliriz.
                Burada ilk yötem ;

                 var productQuery = _productReadRepository.Table.Include(p => p.ProductImageFiles).AsQueryable();

                 yada

                 var test = _productReadRepository.GetAll(false).Include(p => p.ProductImageFiles);

                    Şeklinde kullanabiliriz.


                */
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
                    ProductImageFiles = p.ProductImageFiles.Where(pif => pif.Showcase),
                    p.CreateTime,
                    p.UpdateTime
                })
                .ToListAsync();
                // JSON dönüşümü için liste haline getiriyoruz


                bool hasNextPage = request.Page < totalPageSize - 1;
                bool hasPrevPage = request.Page > 0;
                _logger.LogInformation("Başarılı bir şekilde ürünler listelendi");
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
                _logger.LogError("Ürün listelerken bir hata oluştu.");
                return new GetAllProductsQueryResponse()
                {

                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = ex.Message.ToString()
                };
            }

        }


    }
}

