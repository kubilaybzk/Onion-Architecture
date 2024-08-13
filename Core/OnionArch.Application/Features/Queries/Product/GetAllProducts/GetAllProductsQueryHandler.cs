
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Features.Queries.Product.Product.GetAllProducts;
using OnionArch.Application.View_Models.Category;
using OnionArch.Application.View_Models.Product;
using OnionArch.Domain.Entities;

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
                var productQuery = _productReadRepository.GetAll(false).Include(p=>p.ProductImageFiles).Include(p => p.Brand);

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
                .Select(p => new VM_Result_ProductLink()
                {
                    AppliedDiscountPrice = p.DiscountPrice,
                    AppliedDiscountRate = p.DiscountRate,
                    //Brand=p.Brand,
                    CategoryLists=null,
                    Condition=p.Condition,
                    Currency=p.Currency,
                    SmallDescription=p.SmallDescription,
                    LongDescription=p.LongDescription,
                    DiscountPrice=p.DiscountPrice,
                    DiscountRate=p.DiscountRate,
                    Id = Guid.Parse(p.ID.ToString()),
                    IsActive=p.IsActive,
                    KDVRate=p.KDVRate,
                    LastPrice=p.LastPrice,
                    MaxOrderQuantity=p.MaxOrderQuantity,
                    MinOrderQuantity=p.MinOrderQuantity,
                    Model = p.Model,
                    Name = p.Name,
                    ProductAttributes = null,
                    ProductCode = p.ProductCode,
                    ProductImageFiles = p.ProductImageFiles.Select(p=>new Domain.Entities.ProductImageFile()
                    {
                        FileName = p.FileName,
                        CreateTime = DateTime.Now,
                        ID=p.ID,
                        Path=p.Path,
                        Showcase=p.Showcase,
                        Storage = p.Storage,
                        UpdateTime=DateTime.Now,
                    }).ToList(),
                    StockQuantity=p.StockQuantity,
                    Tax=p.Tax,
                    UnitPrice = p.UnitPrice,
                    MaterializedProductPath=p.MaterializedProductPath,
                    MaterializedProductPathByName=p.MaterializedProductPathByName,
                    MaterializedProductPathBySlug = p.MaterializedProductPathBySlug
                }
                )
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
                    HassError = false,
                    Message = "Ürünler  başarıyla listelendi.",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),



                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Ürün listelerken bir hata oluştu.");
                return new GetAllProductsQueryResponse()
                {
                    ErrorMessage = ex.Message,
                    HassError = true,
                    Message = "Ürün Listeleme işlemi sırasında bir hata ile karşılaşıldı.",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString(),
                };
            }

        }


    }
}

