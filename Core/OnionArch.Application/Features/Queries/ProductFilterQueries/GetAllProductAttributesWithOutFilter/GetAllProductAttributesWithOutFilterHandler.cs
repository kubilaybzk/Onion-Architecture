using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Features.Queries.ProductAttributesQueries.GetAllProductAttributesWithOutFilter;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Application.View_Models.CategoryFilter;

public class GetProductAttributesByCategoryHandler : IRequestHandler<GetAllProductAttributesWithOutFilterRequest, GetAllProductAttributesWithOutFilterResponse>
{
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly IProductReadRepository _productReadRepository;

    public GetProductAttributesByCategoryHandler(ICategoryReadRepository categoryReadRepository, IProductReadRepository productReadRepository)
    {
        _categoryReadRepository = categoryReadRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<GetAllProductAttributesWithOutFilterResponse> Handle(GetAllProductAttributesWithOutFilterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _categoryReadRepository.GetWhere(c => c.CategorySlug == request.CategorySlug)
                .FirstOrDefaultAsync(cancellationToken);

            if (category == null)
            {
                return new GetAllProductAttributesWithOutFilterResponse
                {
                    HassError = true,
                    Message = "Kategori bulunamadı",
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            var allProductIds = new List<Guid>();
            await CollectProductIdsFromCategory(category.ID, allProductIds, cancellationToken);

            var attributes = await _productReadRepository.GetWhere(p => allProductIds.Contains(p.ID))
                .SelectMany(p => p.ProductAttributes)
                .Select(pa => new
                {
                    AttributeId = pa.AttributeValue.Attribute.ID,
                    AttributeName = pa.AttributeValue.Attribute.Name,
                    AttributeValueId = pa.AttributeValue.ID,
                    AttributeValue = pa.AttributeValue.Value,
                    AttributeValueSlug  = pa.AttributeValue.ValueForSlug,
                    AttributeSlug = pa.AttributeValue.Attribute.NameForSlug
                })
                .Distinct()
                .ToListAsync(cancellationToken);

            var uniqueAttributes = attributes
                .GroupBy(a => new { a.AttributeId, a.AttributeName,a.AttributeSlug })
                .Select(p => new VM_Category_Filters_Result()
                {
                    FilterType = null,
                    AttributeId = p.Key.AttributeId.ToString(),
                    AttributeName = p.Key.AttributeName,
                    AttributeNameSlug = p.Key.AttributeSlug,
                    AttributeValues = p.Select(a=> new VM_Category_FilterValue_Result()
                    {
                        AttributeValue=a.AttributeValue,
                        AttributeValueSlug = a.AttributeValueSlug,
                        AttributeValueId=a.AttributeValueId.ToString(),

                    }).ToList()
                })
                .OrderBy(a=>a.AttributeName)
                .ToList();

            return new GetAllProductAttributesWithOutFilterResponse
            {
                CategoryName = category.CategoryName,
                MaterializedPath = category.MaterializedPath,
                CategoryFilters= uniqueAttributes,
                TotalAttributeCount = uniqueAttributes.Count,
                HassError = false,
                Message = "Ürün özellikleri başarıyla getirildi",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
        catch (Exception ex)
        {
            return new GetAllProductAttributesWithOutFilterResponse
            {
                HassError = true,
                Message = "Ürün özellikleri getirilirken bir hata oluştu",
                ErrorMessage = ex.Message,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }

    private async Task CollectProductIdsFromCategory(Guid categoryId, List<Guid> productIds, CancellationToken cancellationToken)
    {
        var category = await _categoryReadRepository.GetWhere(c => c.ID == categoryId)
            .Include(c => c.Products)
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(cancellationToken);

        if (category != null)
        {
            productIds.AddRange(category.Products.Select(p => p.ID));

            foreach (var subCategory in category.SubCategories)
            {
                await CollectProductIdsFromCategory(subCategory.ID, productIds, cancellationToken);
            }
        }
    }
}