using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Features.Queries.CategoryQueries.GetOnlyCategoryName;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Application.View_Models.Category;
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
            try
            {
                // İlk olarak, ana kategoriyi ve ürünlerini yükleyin
                var category = await _categoryReadRepository.GetWhere(p => p.CategorySlug == request.CategorySlug)
                    .Include(p => p.Products)
                    .ThenInclude(p => p.ProductImageFiles)
                    .FirstOrDefaultAsync(cancellationToken);

                if (category == null)
                {
                    // Kategori bulunamazsa, boş bir yanıt döndürün
                    return new GetProductByCategoryResponse()
                    {
                        Products = new List<VM_Result_ProductLink>()
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
                    CategoryLists = p.Categorys.Select(p=>new VM_Result_CategoryList() {
                        MaterializedPathByName = p.MaterializedPathByName,
                        MaterializedPathBySlug = p.MaterializedPathBySlug,
                        MaterializedPath = p.MaterializedPath
                    }).ToList(),
                    Condition = p.Condition,
                    Currency = p.Currency,
                    Description = p.Description,
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

                }).ToList();

                return new GetProductByCategoryResponse()
                {
                    ErrorMessage = null,
                    HassError = false,
                    Message = "Kategoriye göre ürünleri çekme işlemi başarılı",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                    Products = Result,
                    CategoryName= category.CategoryName,
                    MemorizedPath=category.MaterializedPath,
                    MaterializedPathByName= category.MaterializedPathByName,
                    MaterializedPathBySlug = category.MaterializedPathBySlug
                    
                };
            }
            catch (Exception ex)
            {
                return new GetProductByCategoryResponse()
                {
                    ErrorMessage = ex.Message.ToString(),
                    HassError = true,
                    Message = "Kategoriye göre ürünleri çekme işlemi başarısız",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                };
            }
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

