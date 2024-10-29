using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Features.Queries.Product.GetProductByCategory;
using OnionArch.Application.Features.Queries.ProductAttributesQueries.GetCategoriesFilter;
using OnionArch.Application.Repositories.BrandCrud;
using OnionArch.Application.View_Models.Brands;
using OnionArch.Application.View_Models.Category;
using OnionArch.Application.View_Models.Product;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OnionArch.Application.Features.Queries.BrandQueries.GetAllProductByBrandNameQueries
{
    public class GetAllProductByBrandNameHandler : IRequestHandler<GetAllProductByBrandNameRequest, GetAllProductByBrandNameResponse>
    {
        private readonly IBrandReadRepository _brandReadRepository;

        public GetAllProductByBrandNameHandler(IBrandReadRepository brandReadRepository)
        {
            _brandReadRepository = brandReadRepository;
        }

        public async Task<GetAllProductByBrandNameResponse> Handle(GetAllProductByBrandNameRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _brandReadRepository.GetWhere(p => p.BrandSlug == request.BrandName)
                .Include(p => p.Products)
                .ThenInclude(p => p.ProductImageFiles)
                .AsQueryable();


                query = query.Where(p => p.BrandSlug == request.BrandName);

                IQueryable<Domain.Entities.Product> Products = query.SelectMany(p => p.Products);

                Brand? brandInformation = await query.Select(br=>new Brand()
                {
                    BrandName=br.BrandName,
                    BrandSlug=br.BrandSlug,
                    ID=br.ID,
                    TotalProductCount=br.TotalProductCount,
                    DetailTitle=br.DetailTitle,
                    DetailDescription= br.DetailDescription,
                    SeoDetailDescription = br.SeoDetailDescription,
                    SeoLinkDescription = br.SeoLinkDescription,
                    SeoDetailTitle = br.SeoDetailTitle,
                    SeoLinkTitle = br.SeoLinkTitle,
                    BrandLogo = br.BrandLogo,
                     
                }).SingleOrDefaultAsync();
                
                // Öznitelik filtresini uygulayalım
                if (request.AttributeFilters?.Any() == true)
                {
                    Products = ApplyAttributeFilters(Products, request.AttributeFilters);
                }

                // Fiyat aralığı filtresini uygulayalım
                if (request.MinPrice.HasValue || request.MaxPrice.HasValue)
                {
                    Products = ApplyPriceRangeFilter(Products, request.MinPrice, request.MaxPrice);
                }

                // Sıralama uygulayalım
                Products = ApplySorting(Products, request.SortOption);

                // Toplam sayıyı alalım
                var totalCount = await Products.CountAsync(cancellationToken);

                // Sayfalama uygulayalım
                var paginatedProducts = await Products
                    .Skip(request.PaginationValues.Page * request.PaginationValues.Size)
                    .Take(request.PaginationValues.Size)
                    .ToListAsync(cancellationToken);

                // Ürün bilgilerini VM_Result_ProductLink'e dönüştürelim
                var result = paginatedProducts.Select(p=> MapProductToViewModel(p, brandInformation)).ToList();

                // Sayfalama bilgilerini hesaplayalım
                var totalPages = (int)Math.Ceiling(totalCount / (double)request.PaginationValues.Size);
                var hasNext = request.PaginationValues.Page < totalPages - 1;
                var hasPrev = request.PaginationValues.Page > 0;

                brandInformation.Products = null;

                return new GetAllProductByBrandNameResponse()
                {
                    ErrorMessage = null,
                    HassError = false,
                    Message = "Kategoriye göre ürünleri çekme işlemi başarılı",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                    Products = result,
                    TotalCount = totalCount,
                    TotalPageSize = totalPages,
                    CurrentPage = request.PaginationValues.Page,
                    HasNext = hasNext,
                    HasPrev = hasPrev,
                    PageSize = request.PaginationValues.Size,
                    BrandInfo=brandInformation


                };



            }
            catch(Exception ex)
            {
                return new GetAllProductByBrandNameResponse()
                {
                    HassError = true,
                    Message = "Hata alındı",
                    ErrorMessage = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString()
                };
            }
        }


        // Öznitelik filtrelerini uygulayan yardımcı metod
        private IQueryable<OnionArch.Domain.Entities.Product> ApplyAttributeFilters(IQueryable<OnionArch.Domain.Entities.Product> query, Dictionary<string, List<string>> attributeFilters)
        {
            foreach (var filter in attributeFilters)
            {
                var attributeName = filter.Key;
                var attributeValues = filter.Value.SelectMany(v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();

                query = query.Where(p => p.ProductAttributes.Any(pa =>
                    pa.AttributeValue.Attribute.NameForSlug == attributeName &&
                    attributeValues.Contains(pa.AttributeValue.ValueForSlug)));
            }
            return query;
        }

        // Fiyat aralığı filtresini uygulayan yardımcı metod
        private IQueryable<OnionArch.Domain.Entities.Product> ApplyPriceRangeFilter(IQueryable<OnionArch.Domain.Entities.Product> query, decimal? minPrice, decimal? maxPrice)
        {
            if (minPrice.HasValue)
                query = query.Where(p => p.LastPrice >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(p => p.LastPrice <= maxPrice.Value);
            return query;
        }

        // Sıralama uygulayan yardımcı metod
        private IQueryable<OnionArch.Domain.Entities.Product> ApplySorting(IQueryable<OnionArch.Domain.Entities.Product> query, ProductSortOption sortOption)
        {
            return sortOption switch
            {
                ProductSortOption.DateNew => query.OrderByDescending(p => p.CreateTime),
                ProductSortOption.DateOld => query.OrderBy(p => p.CreateTime),
                ProductSortOption.NameAsc => query.OrderBy(p => p.Name),
                ProductSortOption.NameDesc => query.OrderByDescending(p => p.Name),
                ProductSortOption.PriceAsc => query.OrderBy(p => p.LastPrice),
                ProductSortOption.PriceDesc => query.OrderByDescending(p => p.LastPrice),
                _ => query.OrderByDescending(p => p.CreateTime), // Varsayılan sıralama
            };
        }

        // Ürün nesnesini ViewModel'e dönüştüren yardımcı metod
        private VM_Result_ProductLink MapProductToViewModel(OnionArch.Domain.Entities.Product p, Brand brandInformation)
        {
            return new VM_Result_ProductLink
            {
                AppliedDiscountPrice = p.DiscountPrice,
                AppliedDiscountRate = p.DiscountRate,
                Brand = new VM_BrandNameWithId_Result()
                {
                    BrandName = brandInformation.BrandName,
                    BrandSlug = brandInformation.BrandSlug,
                    Id = brandInformation.ID,
                },

                Condition = p.Condition,
                Currency = p.Currency,
                SmallDescription = p.SmallDescription,
                LongDescription = p.LongDescription,
                DiscountPrice = p.DiscountPrice,
                DiscountRate = p.DiscountRate,
                Id = p.ID,
                IsActive = p.IsActive,
                KDVRate = p.KDVRate,
                LastPrice = p.LastPrice,
                MaxOrderQuantity = p.MaxOrderQuantity,
                MinOrderQuantity = p.MinOrderQuantity,
                Model = p.Model,
                Name = p.Name,
                ProductAttributes = null, // Eğer gerekiyorsa, burada ProductAttributes'ı da doldurabilirsiniz
                ProductCode = p.ProductCode,
                ProductImageFiles = p.ProductImageFiles.Select(pif => new OnionArch.Domain.Entities.ProductImageFile
                {
                    FileName = pif.FileName,
                    CreateTime = pif.CreateTime,
                    ID = pif.ID,
                    Path = pif.Path,
                    Showcase = pif.Showcase,
                    Storage = pif.Storage,
                    UpdateTime = pif.UpdateTime,
                }).ToList(),
                StockQuantity = p.StockQuantity,
                Tax = p.Tax,
                UnitPrice = p.UnitPrice,
                MaterializedProductPath = p.MaterializedProductPath,
                MaterializedProductPathByName = p.MaterializedProductPathByName,
                MaterializedProductPathBySlug = p.MaterializedProductPathBySlug
            };
        }


    }
}











/*
  

using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Features.Queries.ProductAttributesQueries.GetCategoriesFilter;
using OnionArch.Application.Repositories.BrandCrud;
using OnionArch.Application.View_Models.Brands;
using OnionArch.Application.View_Models.Category;
using OnionArch.Application.View_Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OnionArch.Application.Features.Queries.BrandQueries.GetAllProductByBrandNameQueries
{
    public class GetAllProductByBrandNameHandler : IRequestHandler<GetAllProductByBrandNameRequest, GetAllProductByBrandNameResponse>
    {
        private readonly IBrandReadRepository _brandReadRepository;

        public GetAllProductByBrandNameHandler(IBrandReadRepository brandReadRepository)
        {
            _brandReadRepository = brandReadRepository;
        }

        public async Task<GetAllProductByBrandNameResponse> Handle(GetAllProductByBrandNameRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var currentBrand = await _brandReadRepository.GetWhere(p => p.BrandSlug == request.BrandName)
                    .Include(p => p.Products)
                    .ThenInclude(p => p.ProductImageFiles)
                    .ToListAsync();

                if (!currentBrand.Any())
                {
                    return new GetAllProductByBrandNameResponse()
                    {
                        HassError = true,
                        Message = "Marka bulunamadı",
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        StatusCodeString = System.Net.HttpStatusCode.NotFound.ToString()
                    };
                }

                var brand = currentBrand.First();
                var products = brand.Products.ToList();

                // Sayfalama için hesaplamalar
                int pageSize = request.PaginationValues.Size;
                int totalCount = products.Count;
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                int currentPage = request.PaginationValues.Page;
                var hasNext = request.PaginationValues.Page < totalPages - 1;
                var hasPrev = request.PaginationValues.Page > 0;
                // Sayfa sınırlarını kontrol et
                if (currentPage < 0) currentPage = 0;
                if (currentPage >= totalPages) currentPage = totalPages - 1;

                // Sayfalanmış ürünleri al
                var pagedProducts = products
                    .Skip(currentPage * pageSize)
                    .Take(pageSize)
                    .Select(p => new VM_Result_ProductLink
                    {
                        Id = p.ID,
                        Name = p.Name,
                        AppliedDiscountPrice = p.AppliedDiscountPrice,
                        AppliedDiscountRate = p.AppliedDiscountRate,
                        Brand = new VM_BrandNameWithId_Result()
                        {
                            BrandName = p.Brand.BrandName,
                            BrandSlug = p.Brand.BrandSlug,
                            Id = p.Brand.ID,
                        },
                        ProductImageFiles = p.ProductImageFiles.Select(pif => new OnionArch.Domain.Entities.ProductImageFile
                        {
                            FileName = pif.FileName,
                            CreateTime = pif.CreateTime,
                            ID = pif.ID,
                            Path = pif.Path,
                            Showcase = pif.Showcase,
                            Storage = pif.Storage,
                            UpdateTime = pif.UpdateTime,
                        }).ToList(),
                        Condition = p.Condition,
                        Currency = p.Currency,
                        DiscountPrice = p.DiscountPrice,
                        DiscountRate = p.DiscountRate,
                        IsActive = p.IsActive,
                        KDVRate = p.KDVRate,
                        LastPrice = p.LastPrice,
                        LongDescription = p.LongDescription,
                        MaterializedProductPath = p.MaterializedProductPath,
                        MaterializedProductPathByName = p.MaterializedProductPathByName,
                        MaterializedProductPathBySlug = p.MaterializedProductPathBySlug,
                        MaxOrderQuantity = p.MaxOrderQuantity,
                        MinOrderQuantity = p.MinOrderQuantity,
                        Model = p.Model,
                        ProductCode = p.ProductCode,
                        Tax = p.Tax,
                        StockQuantity = p.StockQuantity,
                        SmallDescription = p.SmallDescription,
                        
                        // Diğer gerekli product property'leri buraya eklenebilir
                    })
                    .ToList();

                return new GetAllProductByBrandNameResponse()
                {
                    HassError = false,
                    HasPrev = hasPrev,
                    HasNext = hasNext,
                    CurrentPage = currentPage,
                    ErrorMessage = null,
                    Message = "Ürünler başarıyla listelendi",
                    PageSize = pageSize,
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                    TotalCount = totalCount,
                    TotalPageSize = totalPages,
                    Products = pagedProducts
                };
            }
            catch (Exception ex)
            {
                return new GetAllProductByBrandNameResponse()
                {
                    HassError = true,
                    Message = "Hata alındı",
                    ErrorMessage = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString()
                };
            }
        }
    }
}


 */