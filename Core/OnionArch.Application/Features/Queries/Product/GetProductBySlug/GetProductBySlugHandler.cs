using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.View_Models.Product;
using System.Net;

namespace OnionArch.Application.Features.Queries.Product.GetProductBySlug
{
    public class GetProductBySlugHandler : IRequestHandler<GetProductBySlugRequest, GetProductBySlugResponse>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly ILogger<GetProductBySlugHandler> _logger;
        public GetProductBySlugHandler(IProductReadRepository productReadRepository, ILogger<GetProductBySlugHandler> logger)
        {
            _productReadRepository = productReadRepository;
            _logger = logger;
        }

        public async Task<GetProductBySlugResponse> Handle(GetProductBySlugRequest request, CancellationToken cancellationToken)
        {
            try
            {

                var currentProduct =  await _productReadRepository.Table
                    .Include(p=>p.ProductImageFiles)
                    .Where(p => p.MaterializedProductPath == request.MaterializedProductPath)
                    .Select(p=>new VM_Result_ProductLink()
                    {
                        AppliedDiscountPrice = p.DiscountPrice,
                        AppliedDiscountRate = p.DiscountRate,
                        Brand = p.Brand,
                        CategoryLists = null,
                        Condition = p.Condition,
                        Currency = p.Currency,
                        SmallDescription = p.SmallDescription,
                        LongDescription = p.LongDescription,
                        DiscountPrice = p.DiscountPrice,
                        DiscountRate = p.DiscountRate,
                        Id = Guid.Parse(p.ID.ToString()),
                        IsActive = p.IsActive,
                        KDVRate = p.KDVRate,
                        LastPrice = p.LastPrice,
                        MaxOrderQuantity = p.MaxOrderQuantity,
                        MinOrderQuantity = p.MinOrderQuantity,
                        Model = p.Model,
                        Name = p.Name,
                        ProductAttributes = null,
                        ProductCode = p.ProductCode,
                        ProductImageFiles = p.ProductImageFiles.Select(p => new Domain.Entities.ProductImageFile()
                        {
                            FileName = p.FileName,
                            CreateTime = DateTime.Now,
                            ID = p.ID,
                            Path = p.Path,
                            Showcase = p.Showcase,
                            Storage = p.Storage,
                            UpdateTime = DateTime.Now,
                        }).ToList(),
                        StockQuantity = p.StockQuantity,
                        Tax = p.Tax,
                        UnitPrice = p.UnitPrice,
                        MaterializedProductPath = p.MaterializedProductPath,
                        MaterializedProductPathByName = p.MaterializedProductPathByName,
                        MaterializedProductPathBySlug = p.MaterializedProductPathBySlug,

                    })
                    .FirstOrDefaultAsync();

                _logger.LogInformation($"GetProductBySlugResponse Çalıştı {request.MaterializedProductPath}");
                Console.WriteLine($"GetProductBySlugResponse Çalıştı {request.MaterializedProductPath}");

                if (currentProduct != null)
                {
                    return new GetProductBySlugResponse()
                    {
                        ErrorMessage = "",
                        HassError = false,
                        Message = "Ürün başarıyla bulundu",
                        Product = currentProduct,
                        StatusCode = System.Net.HttpStatusCode.OK,
                        StatusCodeString = HttpStatusCode.OK.ToString(),
                    };
                }
                else
                {
                    return new GetProductBySlugResponse()
                    {
                        ErrorMessage = "",
                        HassError = false,
                        Message = "Ürün bulunamadı",
                        Product = null,
                        StatusCode = System.Net.HttpStatusCode.OK,
                        StatusCodeString = HttpStatusCode.OK.ToString(),
                    };
                }
            }
            catch (Exception ex) {
                _logger.LogInformation("GetProductBySlugResponse Çalıştı ve Error verdi");
                Console.WriteLine("GetProductBySlugResponse Çalıştı ve Error verdi");
                return new GetProductBySlugResponse()
                {
                    ErrorMessage = ex.Message.ToString(),
                    HassError = true,
                    Message = "GetProductBySlug backend hatası",
                    Product = null,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                };
            }
        }
    }
}
