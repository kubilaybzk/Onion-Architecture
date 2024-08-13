using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnionArch.Application.Abstractions.AttributeServices;
using OnionArch.Application.Abstractions.CategoryServices;
using OnionArch.Application.Abstractions.HubServices;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Features.Commands.Product.CreateOneProductWithImage;
using OnionArch.Application.Repositories.BrandCrud;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Domain.Entities;
using System.Text.RegularExpressions;

public class CreateOneProductWithImageHandle : IRequestHandler<CreateOneProductWithImageRequest, CreateOneProductWithImageResponse>
{
    private readonly IStorageService _storageService;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly ILogger<CreateOneProductWithImageHandle> _logger;
    private readonly IProductHubService _productHubService;
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly IBrandReadRepository _brandReadRepository;


    public CreateOneProductWithImageHandle(IStorageService storageService,
                                            ILogger<CreateOneProductWithImageHandle> logger,
                                            IProductHubService productHubService,
                                            IProductWriteRepository productWriteRepository,
                                            ICategoryReadRepository categoryReadRepository,
                                            IBrandReadRepository brandReadRepository)
    {
        _storageService = storageService;
        _productWriteRepository = productWriteRepository;
        _logger = logger;
        _productHubService = productHubService;
        _categoryReadRepository = categoryReadRepository;
        _brandReadRepository = brandReadRepository;
    }

    public async Task<CreateOneProductWithImageResponse> Handle(CreateOneProductWithImageRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _storageService.UploadAsync("product-images", request.ImageFiles);
            //var category = await _categoryReadRepository.GetWhere(c => c.ID == Guid.Parse(request.Categories)).FirstOrDefaultAsync(); //ÜRÜNÜN KATEGORİSİNİ BELİRTMEK İÇİN.

            List<Category> categories = new List<Category>();
            foreach (var item in request.Categories)
            {
                var category = await _categoryReadRepository.GetWhere(c => c.ID == Guid.Parse(item)).FirstOrDefaultAsync();
                if (category != null)
                {
                    categories.Add(category);
                }
            }

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
            finalPrice = finalPrice - (finalPrice * (kdvRate / 100));

            // Tax (vergi) oranını belirleme
            decimal? taxRateFromRequest = request.Tax;
            decimal taxRate = taxRateFromRequest ?? 20m; // Varsayılan vergi oranı %20 olarak belirlenmiştir.

            // Son fiyatı vergi uygulandıktan sonra hesaplama
            finalPrice = finalPrice + (finalPrice * (taxRate / 100));
            var ProductsBrand = await _brandReadRepository.GetByIdAsync(request.Brand);
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
                Currency = request.Currency ?? "TRY", // Türk Lirası
                Condition = ValidateCondition(request.Condition), // Durumun doğrulanması
                SmallDescription = request.SmallDescription,
                LongDescription = request.LongDescription,
                Categorys = categories,
                IsActive = request.IsActive,
                MaxOrderQuantity = request.MaxOrderQuantity,
                MinOrderQuantity = request.MinOrderQuantity,
                ProductCode = request.ProductCode,
                StockQuantity = request.StockQuantity,
                Model = request.Model,
                Brand= ProductsBrand,
                MaterializedProductPath = GenerateSlug(string.Concat(ProductsBrand.BrandName, "-", request.Name)),
                MaterializedProductPathByName = string.Concat(categories[0].MaterializedPathByName, ".", string.Concat(ProductsBrand.BrandName, "-", request.Name)),
                MaterializedProductPathBySlug = string.Concat(categories[0].MaterializedPathBySlug, ".", GenerateSlug(string.Concat(ProductsBrand.BrandName, "-", request.Name))),

            };
            
            await _productWriteRepository.AddAsync(product);
            _logger.LogInformation("Ürün ekleme işlemi başarılı");
            await _productHubService.ProductAddOperationMessage("Ürün listesine bir adet ürün eklendi");
            await _productWriteRepository.SaveAsync();

            return new CreateOneProductWithImageResponse
            {
                isCreated = true,
                ErrorMessage = "",
                HassError = false,
                Message = "Ürün ekleme işlemi başarıyla gerçekleşti",
                StatusCode = System.Net.HttpStatusCode.Created,
                StatusCodeString = System.Net.HttpStatusCode.Created.ToString(),
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ürün eklenirken bir sorun ile karşılaşıldı: {ex.Message}");
            return new CreateOneProductWithImageResponse
            {
                isCreated = false,
                ErrorMessage = ex.Message,
                HassError = true,
                Message = "Ürün ekleme işlemi sırasında bir hata ile karşılaşıldı.",
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
    public string GenerateSlug(string phrase)
    {
        // Türkçe karakterleri çıkar
        string str = RemoveTurkishCharacters(phrase).ToLower();

        // Geçersiz karakterleri temizle
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        // Birden fazla boşluğu tek boşluğa dönüştür
        str = Regex.Replace(str, @"\s+", " ").Trim();
        // 45 karakteri aşmayacak şekilde kırp ve boşlukları kes
        //str = str.Substring(0, Math.Min(str.Length, 45)).Trim();
        // Boşlukları tireye dönüştür
        str = Regex.Replace(str, @"\s", "-");

        return str;
    }

    public string RemoveTurkishCharacters(string input)
    {
        // Türkçe karakterleri çevirme
        input = input.Replace("ı", "i").Replace("İ", "I")
                     .Replace("ş", "s").Replace("Ş", "S")
                     .Replace("ğ", "g").Replace("Ğ", "G")
                     .Replace("ç", "c").Replace("Ç", "C")
                     .Replace("ö", "o").Replace("Ö", "O")
                     .Replace("ü", "u").Replace("Ü", "U");

        return input;
    }
}
