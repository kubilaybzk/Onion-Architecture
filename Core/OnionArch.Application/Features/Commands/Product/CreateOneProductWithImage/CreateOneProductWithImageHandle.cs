using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnionArch.Application.Abstractions.AttributeServices;
using OnionArch.Application.Abstractions.HubServices;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Features.Commands.Product.CreateOneProductWithImage;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Domain.Entities;

public class CreateOneProductWithImageHandle : IRequestHandler<CreateOneProductWithImageRequest, CreateOneProductWithImageResponse>
{
    private readonly IStorageService _storageService;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly ILogger<CreateOneProductWithImageHandle> _logger;
    private readonly IProductHubService _productHubService;
    private readonly ICategoryReadRepository _categoryReadRepository;

    public CreateOneProductWithImageHandle(IStorageService storageService,
                                            ILogger<CreateOneProductWithImageHandle> logger,
                                            IProductHubService productHubService,
                                            IProductWriteRepository productWriteRepository,
                                            ICategoryReadRepository categoryReadRepository)
    {
        _storageService = storageService;
        _productWriteRepository = productWriteRepository;
        _logger = logger;
        _productHubService = productHubService;
        _categoryReadRepository = categoryReadRepository;
   
    }

    public async Task<CreateOneProductWithImageResponse> Handle(CreateOneProductWithImageRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _storageService.UploadAsync("product-images", request.ImageFiles);
            var category = await _categoryReadRepository.GetWhere(c => c.ID == Guid.Parse(request.Category)).FirstOrDefaultAsync(); //ÜRÜNÜN KATEGORİSİNİ BELİRTMEK İÇİN.
            // İndirim oranı ve fiyatının uygulanması
            decimal appliedDiscountRate = 0;
            decimal appliedDiscountPrice = 0;
            decimal finalPrice = request.UnitPrice;

            if (request.DiscountRate > 0 && request.DiscountPrice > 0)
            {
                // Hem yüzdesel indirim hem de birim indirimi uygulanmışsa
                appliedDiscountRate = request.DiscountRate;
                decimal priceAfterRateDiscount = request.UnitPrice - (request.UnitPrice * appliedDiscountRate / 100);
                finalPrice = priceAfterRateDiscount - request.DiscountPrice;
            }
            
            else if (request.DiscountRate > 0)
            {
                // Yalnızca yüzdesel indirim uygulanmışsa
                appliedDiscountRate = request.DiscountRate;
                finalPrice = request.UnitPrice - (request.UnitPrice * appliedDiscountRate / 100);
            }
            
            else if (request.DiscountPrice > 0)
            {
                // Yalnızca birim indirimi uygulanmışsa
                finalPrice = request.UnitPrice - request.DiscountPrice;
            }

            // KDV oranı
            decimal? kdvRateFromRequest = request.KDVRate;
            decimal kdvRate = kdvRateFromRequest ?? 20m;      // Türkiye'de genellikle KDV oranı %20 olarak uygulanır.

            // Kdv uygulandıktan sonraki son fiyat Vergi miktarı
            finalPrice = finalPrice-(finalPrice * (kdvRate / 100));

            // Tax (vergi) oranını belirleme
            decimal? taxRateFromRequest = request.Tax;
            decimal taxRate = taxRateFromRequest ?? 20m; // Varsayılan vergi oranı %20 olarak belirlenmiştir.

            // Son fiyatı vergi uygulandıktan sonra hesaplama
            finalPrice = finalPrice + (finalPrice * (taxRate / 100));

            var product = new Product
            {
                UnitPrice = request.UnitPrice,
                Name = request.Name,
                ProductImageFiles = result.Select((d, index) => new ProductImageFile
                {
                    FileName = d.fileName,
                    Path = d.PathOrContainerName,
                    Storage = _storageService.StorageType,
                    Showcase = (index == 0)
                }).ToList(),
                DiscountRate = request.DiscountRate,
                DiscountPrice = request.DiscountPrice,
                AppliedDiscountRate = appliedDiscountRate,
                AppliedDiscountPrice = appliedDiscountPrice,
                Tax = taxRate,
                KDVRate = kdvRate,
                LastPrice = finalPrice,
                Currency = request.Currency?? "TRY", // Türk Lirası
                Condition = ValidateCondition(request.Condition), // Durumun doğrulanması
                Brand = request.Brand,
                Description = request.Description,
                Categorys = new List<Category> { category },
                IsActive = request.IsActive,
                MaxOrderQuantity = request.MaxOrderQuantity,
                MinOrderQuantity = request.MinOrderQuantity,
                ProductCode = request.ProductCode,
                StockQuantity = request.StockQuantity,
                Model = request.Model,
                ProductAttributes = new List<ProductAttribute>()
            };

            await _productWriteRepository.AddAsync(product);
            _logger.LogInformation("Başarılı bir şekilde ürün eklendi");
            await _productHubService.ProductAddOperationMessage("Ürün listesine bir adet ürün eklendi");
            await _productWriteRepository.SaveAsync();

            return new CreateOneProductWithImageResponse
            {
                ErrorMessage = "",
                HassError = false,
                Message = "Ekleme başarıyla gerçekleşmiştir.",
                StatusCode = System.Net.HttpStatusCode.Created,
                StatusCodeString = System.Net.HttpStatusCode.Created.ToString(),
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ürün eklenirken bir sorun ile karşılaşıldı: {ex.Message}");
            return new CreateOneProductWithImageResponse
            {
                ErrorMessage = ex.Message,
                HassError = true,
                Message = "Ekleme işlemi sırasında bir hata ile karşılaşıldı.",
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                StatusCodeString = System.Net.HttpStatusCode.BadRequest.ToString(),
            };
        }
    }

    // Condition değerinin doğrulanması
    private string ValidateCondition(string condition)
    {
        // Condition sadece belirli değerleri alabilir: "Yeni", "Kullanılmış", "Yenilenmiş"
        string[] validConditions = { "Yeni", "Kullanılmış", "Yenilenmiş" };
        return validConditions.Contains(condition) ? condition : "Yeni"; // Varsayılan değer "Yeni" olarak ayarlanır
    }
}
