using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnionArch.Application.Abstractions.CategoryServices;
using OnionArch.Application.Abstractions.HubServices;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OnionArch.Application.Features.Commands.Product.UpdateProductByIDCommands
{
    public class UpdateProductByIDCommandsHandler : IRequestHandler<UpdateProductByIDCommandsRequest, UpdateProductByIDCommandsResponse>
    {
        private readonly IStorageService _storageService;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly ILogger<CreateOneProductWithImageHandle> _logger;
        private readonly IProductHubService _productHubService;
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly IProductReadRepository _productReadRepository;
        public UpdateProductByIDCommandsHandler(IStorageService storageService, IProductWriteRepository productWriteRepository, ILogger<CreateOneProductWithImageHandle> logger, IProductHubService productHubService, ICategoryReadRepository categoryReadRepository, IProductReadRepository productReadRepository)
        {
            _storageService = storageService;
            _productWriteRepository = productWriteRepository;
            _logger = logger;
            _productHubService = productHubService;
            _categoryReadRepository = categoryReadRepository;
            _productReadRepository = productReadRepository;
        }


        public async Task<UpdateProductByIDCommandsResponse> Handle(UpdateProductByIDCommandsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var Product = await _productReadRepository.Table.Include(p=>p.Categorys).Include(p=>p.ProductImageFiles).FirstOrDefaultAsync(p=>p.ID==Guid.Parse(request.Id));


                if(request.ImageFiles?.Count>0)
                {
                    var result = await _storageService.UploadAsync("product-images", request.ImageFiles);
                    Product.ProductImageFiles = result.Select((d, index) => new ProductImageFile
                    {
                        FileName = d.fileName,
                        Path = d.PathOrContainerName,
                        Storage = _storageService.StorageType,
                        Showcase = (index == 0)
                    }).ToList();
                }



                List<Category> categories = new List<Category>();
                foreach (var item in request.Categories)
                {
                    var category = await _categoryReadRepository.GetWhere(c => c.ID == Guid.Parse(item)).FirstOrDefaultAsync();
                    if (category != null)
                    {
                        categories.Add(category);
                    }
                }


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


                Product.UnitPrice = request.UnitPrice;
                Product.Name = request.Name;
                Product.DiscountRate = request.DiscountRate;
                Product.DiscountPrice = request.DiscountPrice;
                Product.AppliedDiscountRate = appliedDiscountRate;
                Product.AppliedDiscountPrice = appliedDiscountPrice;
                Product.Tax = taxRate;
                Product.KDVRate = kdvRate;
                Product.LastPrice = finalPrice;
                Product.Currency = request.Currency ?? "TRY"; // Türk Lirası
                Product.Condition = ValidateCondition(request.Condition); // Durumun doğrulanması
                Product.Brand = request.Brand;
                Product.SmallDescription = request.SmallDescription;
                Product.LongDescription = request.LongDescription;
                Product.IsActive = request.IsActive;
                Product.MaxOrderQuantity = request.MaxOrderQuantity;
                Product.MinOrderQuantity = request.MinOrderQuantity;
                Product.ProductCode = request.ProductCode;
                Product.StockQuantity = request.StockQuantity;
                Product.Model = request.Model;
                Product.Categorys = categories;
                Product.MaterializedProductPath =       GenerateSlug(string.Concat(request.Brand, "-", request.Name));
                Product.MaterializedProductPathByName = string.Concat(categories[0].MaterializedPathByName, ".", string.Concat(request.Brand, "-", request.Name));
                Product.MaterializedProductPathBySlug = string.Concat(categories[0].MaterializedPathBySlug, ".", GenerateSlug(string.Concat(request.Brand, "-", request.Name)));

                _logger.LogInformation("Ürün güncelleme işlemi başarılı");
                await _productWriteRepository.SaveAsync();

                return new UpdateProductByIDCommandsResponse()
                {
                    isUpdated = true,
                    ErrorMessage = "",
                    HassError = false,
                    Message = "ürün başarıyla güncellendi",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = System.Net.HttpStatusCode.OK.ToString()
                };
            }
            
            catch (Exception ex)
            {
                return new UpdateProductByIDCommandsResponse()
                {
                    isUpdated = false,
                    ErrorMessage = ex.Message.ToString(),
                    HassError = true,
                    Message = "ürün güncellerken hata ",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString()
                };
            }

        }
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
    
}
