using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Features.Queries.Product.GetProductByCategory;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Application.View_Models.Brands;
using OnionArch.Application.View_Models.Category;
using OnionArch.Application.View_Models.Product;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GetProductByCategoryHandler : IRequestHandler<GetProductByCategoryRequest, GetProductByCategoryResponse>
{
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly IProductReadRepository _productReadRepository;

    public GetProductByCategoryHandler(ICategoryReadRepository categoryReadRepository, IProductReadRepository productReadRepository)
    {
        _categoryReadRepository = categoryReadRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<GetProductByCategoryResponse> Handle(GetProductByCategoryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Tüm ürünleri ve gerekli ilişkileri tek sorguda yükleyelim
            var query = _productReadRepository.Table
                .Include(p => p.Categorys)
                .Include(p=>p.Brand)
                .Include(p => p.ProductImageFiles)
                .Include(p => p.ProductAttributes)
                    .ThenInclude(pa => pa.AttributeValue)
                        .ThenInclude(av => av.Attribute)
                .AsQueryable();

            // Kategori filtresini uygulayalım
            // Bu, ana kategori ve tüm alt kategorilerdeki ürünleri getirir
            query = query.Where(p => p.Categorys.Any(c =>
                c.CategorySlug == request.CategorySlug ||
                c.MaterializedPathBySlug.Contains(request.CategorySlug)));

            // Öznitelik filtresini uygulayalım
            if (request.AttributeFilters?.Any() == true)
            {
                query = ApplyAttributeFilters(query, request.AttributeFilters);
            }

            // Fiyat aralığı filtresini uygulayalım
            if (request.MinPrice.HasValue || request.MaxPrice.HasValue)
            {
                query = ApplyPriceRangeFilter(query, request.MinPrice, request.MaxPrice);
            }

            // Sıralama uygulayalım
            query = ApplySorting(query, request.SortOption);

            // Toplam sayıyı alalım
            var totalCount = await query.CountAsync(cancellationToken);

            // Sayfalama uygulayalım
            var paginatedProducts = await query
                .Skip(request.PaginationValues.Page * request.PaginationValues.Size)
                .Take(request.PaginationValues.Size)
                .ToListAsync(cancellationToken);

            // Ürün bilgilerini VM_Result_ProductLink'e dönüştürelim
            var result = paginatedProducts.Select(MapProductToViewModel).ToList();

            // Sayfalama bilgilerini hesaplayalım
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PaginationValues.Size);
            var hasNext = request.PaginationValues.Page < totalPages - 1;
            var hasPrev = request.PaginationValues.Page > 0;

            // Kategori bilgilerini alalım
            var category = await _categoryReadRepository.GetWhere(c => c.CategorySlug == request.CategorySlug)
                .FirstOrDefaultAsync(cancellationToken);

            // Sonuç nesnesini oluşturalım ve döndürelim
            return new GetProductByCategoryResponse
            {
                ErrorMessage = null,
                HassError = false,
                Message = "Kategoriye göre ürünleri çekme işlemi başarılı",
                StatusCode = System.Net.HttpStatusCode.OK,
                StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                Products = result,
                CategoryName = category?.CategoryName,
                MemorizedPath = category?.MaterializedPath,
                MaterializedPathByName = category?.MaterializedPathByName,
                MaterializedPathBySlug = category?.MaterializedPathBySlug,
                TotalCount = totalCount,
                TotalPageSize = totalPages,
                CurrentPage = request.PaginationValues.Page,
                HasNext = hasNext,
                HasPrev = hasPrev,
                PageSize = request.PaginationValues.Size
            };
        }
        catch (Exception ex)
        {
            // Hata durumunda hata bilgilerini içeren yanıt döndür
            return new GetProductByCategoryResponse
            {
                ErrorMessage = ex.Message,
                HassError = true,
                Message = "Kategoriye göre ürünleri çekme işlemi başarısız",
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString(),
            };
        }
    }

    // Öznitelik filtrelerini uygulayan yardımcı metod
    private IQueryable<OnionArch.Domain.Entities.Product> ApplyAttributeFilters(IQueryable<OnionArch.Domain.Entities.Product> query,Dictionary<string, List<string>> attributeFilters)
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
    private IQueryable<OnionArch.Domain.Entities.Product> ApplyPriceRangeFilter( IQueryable<OnionArch.Domain.Entities.Product> query,decimal? minPrice,decimal? maxPrice)
    {
        if (minPrice.HasValue)
            query = query.Where(p => p.LastPrice >= minPrice.Value);
        if (maxPrice.HasValue)
            query = query.Where(p => p.LastPrice <= maxPrice.Value);
        return query;
    }

    // Sıralama uygulayan yardımcı metod
    private IQueryable<OnionArch.Domain.Entities.Product> ApplySorting( IQueryable<OnionArch.Domain.Entities.Product> query, ProductSortOption sortOption)
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
    private VM_Result_ProductLink MapProductToViewModel(OnionArch.Domain.Entities.Product p)
    {
        return new VM_Result_ProductLink
        {
            AppliedDiscountPrice = p.DiscountPrice,
            AppliedDiscountRate = p.DiscountRate,
            Brand = new VM_BrandNameWithId_Result()
            {
                BrandName = p.Brand.BrandName,
                BrandSlug = p.Brand.BrandSlug,
                Id = p.Brand.ID,
            },
            CategoryLists = p.Categorys.Select(c => new VM_Result_CategoryList
            {
                MaterializedPathByName = c.MaterializedPathByName,
                MaterializedPathBySlug = c.MaterializedPathBySlug,
                MaterializedPath = c.MaterializedPath
            }).ToList(),
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

































/*




using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Features.Queries.Product.GetProductByCategory;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Application.View_Models.Category;
using OnionArch.Application.View_Models.Product;
using OnionArch.Domain.Entities;

public class GetProductByCategoryHandler : IRequestHandler<GetProductByCategoryRequest, GetProductByCategoryResponse>
{
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly IProductReadRepository _productReadRepository;

    public GetProductByCategoryHandler(ICategoryReadRepository categoryReadRepository, IProductReadRepository productReadRepository)
    {
        _categoryReadRepository = categoryReadRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<GetProductByCategoryResponse> Handle(GetProductByCategoryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Ana kategoriyi ve ürünlerini yükle
            var category = await _categoryReadRepository.GetWhere(p => p.CategorySlug == request.CategorySlug)
                .Include(p => p.Products)
                    .ThenInclude(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(cancellationToken);

            if (category == null)
            {
                // Kategori bulunamazsa boş yanıt döndür
                return new GetProductByCategoryResponse { Products = new List<VM_Result_ProductLink>() };
            }

            // Tüm ürünleri topla
            var allProducts = new List<OnionArch.Domain.Entities.Product>(category.Products);

            // Alt kategorileri ve ürünlerini yükle
            await LoadSubCategoriesWithProducts(category, allProducts, cancellationToken);

            // Filtreleme işlemi
            if (request.AttributeFilters?.Any() == true)
            {
                var filteredProductIds = await FilterProductsByAttributes(allProducts.Select(p => p.ID).ToList(), request.AttributeFilters, cancellationToken);
                allProducts = allProducts.Where(p => filteredProductIds.Contains(p.ID)).ToList();
            }

            allProducts = SortProducts(allProducts, request.SortOption);

            if (request.MinPrice.HasValue || request.MaxPrice.HasValue)
            {
                allProducts = FilterProductsByPriceRange(allProducts, request.MinPrice, request.MaxPrice);
            }

            // Sayfalama işlemi
            var totalCount = allProducts.Count;
            var paginatedProducts = allProducts
                .Skip(request.PaginationValues.Page * request.PaginationValues.Size)
                .Take(request.PaginationValues.Size)
                .ToList();

            // Ürün bilgilerini VM_Result_ProductLink'e dönüştür
            var result = paginatedProducts.Select(MapProductToViewModel).ToList();

            // Sayfalama bilgilerini hesapla
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PaginationValues.Size);
            var hasNext = request.PaginationValues.Page < totalPages - 1;
            var hasPrev = request.PaginationValues.Page > 0;

            // Sonuç nesnesini oluştur ve döndür
            return new GetProductByCategoryResponse
            {
                ErrorMessage = null,
                HassError = false,
                Message = "Kategoriye göre ürünleri çekme işlemi başarılı",
                StatusCode = System.Net.HttpStatusCode.OK,
                StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                Products = result,
                CategoryName = category.CategoryName,
                MemorizedPath = category.MaterializedPath,
                MaterializedPathByName = category.MaterializedPathByName,
                MaterializedPathBySlug = category.MaterializedPathBySlug,
                TotalCount = totalCount,
                TotalPageSize = totalPages,
                CurrentPage = request.PaginationValues.Page,
                HasNext = hasNext,
                HasPrev = hasPrev,
                PageSize = request.PaginationValues.Size
            };
        }
        catch (Exception ex)
        {
            // Hata durumunda hata bilgilerini içeren yanıt döndür
            return new GetProductByCategoryResponse
            {
                ErrorMessage = ex.Message,
                HassError = true,
                Message = "Kategoriye göre ürünleri çekme işlemi başarısız",
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString(),
            };
        }
    }

    // Alt kategorileri ve ürünlerini yükleyen yardımcı metod
    private async Task LoadSubCategoriesWithProducts(Category category, List<OnionArch.Domain.Entities.Product> allProducts, CancellationToken cancellationToken)
    {
        var subCategories = await _categoryReadRepository.GetWhere(c => c.ParentCategoryId == category.ID)
            .Include(sc => sc.Products)
                .ThenInclude(p => p.ProductImageFiles)
            .Include(sc => sc.SubCategories)
            .ToListAsync(cancellationToken);

        foreach (var subCategory in subCategories)
        {
            allProducts.AddRange(subCategory.Products);
            await LoadSubCategoriesWithProducts(subCategory, allProducts, cancellationToken);
        }
    }

    // Ürünleri özelliklere göre filtreleyen metod
    private async Task<List<Guid>> FilterProductsByAttributes(List<Guid> productIds, Dictionary<string, List<string>> attributeFilters, CancellationToken cancellationToken)
    {
        var query = _productReadRepository.Table
            .Where(p => productIds.Contains(p.ID))
            .Include(p => p.ProductAttributes)
                .ThenInclude(pa => pa.AttributeValue)
                    .ThenInclude(av => av.Attribute)
            .AsQueryable();

        foreach (var filter in attributeFilters)
        {
            var attributeName = filter.Key;
            var attributeValues = filter.Value.SelectMany(v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();

            query = query.Where(p => p.ProductAttributes.Any(pa =>
                pa.AttributeValue.Attribute.NameForSlug == attributeName &&
                attributeValues.Contains(pa.AttributeValue.ValueForSlug)));
        }

        var filteredProductIds = await query.Select(p => p.ID).ToListAsync(cancellationToken);

        return filteredProductIds;
    }
   
    // Ürün nesnesini ViewModel'e dönüştüren yardımcı metod
    private VM_Result_ProductLink MapProductToViewModel(OnionArch.Domain.Entities.Product p)
    {
        return new VM_Result_ProductLink
        {
            AppliedDiscountPrice = p.DiscountPrice,
            AppliedDiscountRate = p.DiscountRate,
            Brand = p.Brand,
            CategoryLists = p.Categorys.Select(c => new VM_Result_CategoryList
            {
                MaterializedPathByName = c.MaterializedPathByName,
                MaterializedPathBySlug = c.MaterializedPathBySlug,
                MaterializedPath = c.MaterializedPath
            }).ToList(),
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
            ProductAttributes = null,
            ProductCode = p.ProductCode,
            ProductImageFiles = p.ProductImageFiles.Select(pif => new OnionArch.Domain.Entities.ProductImageFile
            {
                FileName = pif.FileName,
                CreateTime = DateTime.Now,
                ID = pif.ID,
                Path = pif.Path,
                Showcase = pif.Showcase,
                Storage = pif.Storage,
                UpdateTime = DateTime.Now,
            }).ToList(),
            StockQuantity = p.StockQuantity,
            Tax = p.Tax,
            UnitPrice = p.UnitPrice,
            MaterializedProductPath = p.MaterializedProductPath,
            MaterializedProductPathByName = p.MaterializedProductPathByName,
            MaterializedProductPathBySlug = p.MaterializedProductPathBySlug
        };
    }
    
    // Ürünlerin sırlamasını sağlayan yöntem.
    private List<OnionArch.Domain.Entities.Product> SortProducts(List<OnionArch.Domain.Entities.Product> products, ProductSortOption sortOption)
    {
        return sortOption switch
        {
            ProductSortOption.DateNew => products.OrderByDescending(p => p.CreateTime).ToList(),
            ProductSortOption.DateOld => products.OrderBy(p => p.CreateTime).ToList(),
            ProductSortOption.NameAsc => products.OrderBy(p => p.Name).ToList(),
            ProductSortOption.NameDesc => products.OrderByDescending(p => p.Name).ToList(),
            ProductSortOption.PriceAsc => products.OrderBy(p => p.LastPrice).ToList(),
            ProductSortOption.PriceDesc => products.OrderByDescending(p => p.LastPrice).ToList(),
            _ => products.OrderByDescending(p => p.CreateTime).ToList(), // Default sıralama
        };
    }
    
    // Belirli fiyat aralığında olan ürünlerin getirilmesini sağlayan filtre 
    private List<OnionArch.Domain.Entities.Product> FilterProductsByPriceRange(List<OnionArch.Domain.Entities.Product> products, decimal? minPrice, decimal? maxPrice)
    {
        return products.Where(p =>
            (!minPrice.HasValue || p.LastPrice >= minPrice.Value) &&
            (!maxPrice.HasValue || p.LastPrice <= maxPrice.Value)
        ).ToList();
    }

}

 */