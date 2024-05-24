using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Features.Queries.CategoryQueries.GetOnlyCategoryName;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Application.View_Models.Product;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Product.GetProductByCategory
{
    public class GetProductByCategoryHandler : IRequestHandler<GetProductByCategoryRequest, GetProductByCategoryResponse>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;

        public GetProductByCategoryHandler(ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;
        }



        public async Task<GetProductByCategoryResponse> Handle(GetProductByCategoryRequest request, CancellationToken cancellationToken)
        {
            // İlk olarak, ana kategoriyi ve ürünlerini yükleyin
            var category = await _categoryReadRepository.GetWhere(p => p.CategorySlug == request.CategorySlug)
                .Include(p => p.Products)
                .ThenInclude(p=>p.ProductImageFiles)
                .FirstOrDefaultAsync(cancellationToken);

            if (category == null)
            {
                // Kategori bulunamazsa, boş bir yanıt döndürün
                return new GetProductByCategoryResponse()
                {
                    CategoryProducts = new List<VM_Result_ProductLink>()
                };
            }

            // Tüm ürünleri saklamak için bir liste oluşturun
            var allProducts = new List<OnionArch.Domain.Entities.Product>();
            allProducts.AddRange(category.Products);

            // Alt kategorileri ve ürünlerini yüklemek için rekürsif fonksiyonu çağırın
            await LoadSubCategoriesWithProducts(category, allProducts, cancellationToken);

            var Result = allProducts.Select(p => new VM_Result_ProductLink()
            {
                AppliedDiscountPrice = p.DiscountPrice,
                AppliedDiscountRate = p.DiscountRate,
                Brand = p.Brand,
                Condition = p.Condition,
                Currency = p.Currency,
                Description = p.Description,
                DiscountPrice = p.DiscountPrice,
                DiscountRate = p.DiscountRate,
                IsActive = p.IsActive,
                LastPrice = p.LastPrice,
                MaxOrderQuantity = p.MaxOrderQuantity,
                MinOrderQuantity = p.MinOrderQuantity,
                Model=p.Model,
                Name=p.Name,
                ProductCode = p.ProductCode,
                ProductImageFiles = p.ProductImageFiles.Select(p=>new Domain.Entities.ProductImageFile
                {
                    CreateTime = DateTime.Now,
                    Path = p.Path,
                    Showcase = p.Showcase,
                    Storage = p.Storage,
                    FileName = p.FileName,
                    ID = p.ID,
                }).ToList(),
                StockQuantity = p.StockQuantity,
                UnitPrice=p.UnitPrice,
                
            }).ToList();

            return new GetProductByCategoryResponse()
            {
                ErrorMessage = null,
                HassError = false,
                Message = "Kategorileri çekme işlemi başarılı",
                StatusCode = System.Net.HttpStatusCode.OK,
                StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                CategoryProducts = Result
            };
        }

        private async Task LoadSubCategoriesWithProducts(Category category, List<OnionArch.Domain.Entities.Product> allProducts, CancellationToken cancellationToken)
        {
            // Alt kategorileri ve ürünlerini yükleyin
            var subCategories = await _categoryReadRepository.GetWhere(c => c.ParentCategoryId == category.ID)
                .Include(sc => sc.Products)
                  .ThenInclude(p => p.ProductImageFiles)
                .Include(sc=>sc.SubCategories)
                .ToListAsync(cancellationToken);

            // Eğer alt kategoriler yoksa işleme devam etmeyin
            if (subCategories == null || !subCategories.Any())
            {
                return;
            }

            foreach (var subCategory in subCategories)
            {
                // Alt kategorinin ürünlerini listeye ekleyin
                allProducts.AddRange(subCategory.Products);

                // Rekürsif olarak alt kategorilerin alt kategorilerini ve ürünlerini yükleyin
                await LoadSubCategoriesWithProducts(subCategory, allProducts, cancellationToken);
            }
        }


    }
}

