
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Features.Queries.Product.Product.GetAllProducts;
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
                var productQuery = _productReadRepository.GetAll(false).Include(p=>p.ProductImageFiles);

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
                    p.Name,// Ürün adı
                    p.Description,// Ürün açıklaması
                    p.Brand,// Ürün markası
                    p.Model,// Ürün modeli
                    p.Categorys,// Ürün kategorisi
                    p.ProductCode,// Ürün kodu
                    p.UnitPrice,// Birim fiyatı
                    p.DiscountRate,// İndirim oranı
                    p.DiscountPrice,// İndirimli fiyatı
                    p.AppliedDiscountRate,// İndirim oranı uygulanmış hali
                    p.AppliedDiscountPrice,// İndirimli fiyat uygulanmış hali
                    p.Tax,// Vergi miktarı
                    p.KDVRate,// KDV oranı
                    p.LastPrice,//Tüm hesaplamalardan sonraki fiyat
                    p.Currency,// Para birimi
                    p.StockQuantity,// Stok miktarı
                    p.MinOrderQuantity,// Minimum stok seviyesi
                    p.MaxOrderQuantity,// Maksimum stok seviyesi
                    p.Condition,// Ürün durumu (yeni, kullanılmış, yenilenmiş)
                    p.IsActive,// Ürün aktif mi?
                    p.CreateTime,
                    p.UpdateTime,
                    p.ID,
                    p.ProductImageFiles

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

