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
}