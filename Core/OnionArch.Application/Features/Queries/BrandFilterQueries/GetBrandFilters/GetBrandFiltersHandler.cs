using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Features.Queries.ProductAttributesQueries.GetCategoriesFilter;
using OnionArch.Application.Repositories.BrandCrud;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Application.View_Models.CategoryFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.BrandFilterQueries.GetBrandFilters
{
    public class GetBrandFiltersHandler : IRequestHandler<GetBrandFiltersRequest, GetBrandFiltersResponse>
    {
        private readonly IBrandReadRepository   _brandReadRepository;
        private readonly IProductReadRepository _productReadRepository;

        public GetBrandFiltersHandler(IProductReadRepository productReadRepository, IBrandReadRepository brandReadRepository)
        {
            _productReadRepository = productReadRepository;
            _brandReadRepository = brandReadRepository;
        }

        public async Task<GetBrandFiltersResponse> Handle(GetBrandFiltersRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _brandReadRepository.GetWhere(c => c.BrandSlug == request.BrandSlug)
                    .Select(c => new
                    {
                        c.ID,
                        CategoryAttributes = c.BrandAttributes.Select(ca => new
                        {
                            ca.FilterType,
                            ca.FilterId,
                            ca.Order,
                            FilterName = ca.Filter.Name,
                            FilterNameForSlug = ca.Filter.NameForSlug
                        }).OrderBy(ca => ca.Order).ToList()
                    })
                    .SingleOrDefaultAsync(cancellationToken);

                if (category == null)
                {
                    return new GetBrandFiltersResponse
                    {
                        HassError = true,
                        Message = "Marka'ya göre listelenen ürünlerin özellikleri getirilemedi, Marka bulunamadı",
                        StatusCode = System.Net.HttpStatusCode.NotFound
                    };
                }

                var allProductIds = await CollectProductIdsFromCategory(category.ID, cancellationToken);

                var attributes = await _productReadRepository.GetWhere(p => allProductIds.Contains(p.ID))
                    .SelectMany(p => p.ProductAttributes)
                    .Select(pa => new
                    {
                        AttributeId = pa.AttributeValue.Attribute.ID,
                        AttributeValueId = pa.AttributeValue.ID,
                        AttributeValue = pa.AttributeValue.Value,
                        AttributeValueSlug = pa.AttributeValue.ValueForSlug
                    })
                    .Distinct()
                    .ToListAsync(cancellationToken);

                var categoryFilters = category.CategoryAttributes
                    .Select(ca => new VM_Category_Filters_Result
                    {
                        FilterType = ca.FilterType,
                        AttributeId = ca.FilterId.ToString(),
                        AttributeName = ca.FilterName,
                        AttributeNameSlug = ca.FilterNameForSlug,
                        Order = ca.Order,
                        AttributeValues = attributes
                            .Where(a => a.AttributeId == ca.FilterId)
                            .Select(a => new VM_Category_FilterValue_Result
                            {
                                AttributeValue = a.AttributeValue,
                                AttributeValueSlug = a.AttributeValueSlug,
                                AttributeValueId = a.AttributeValueId.ToString(),
                            })
                            .ToList()
                    })
                    .ToList();

                return new GetBrandFiltersResponse
                {
                    BrandFilters = categoryFilters,
                    Message = "Marka'ya göre listelenen ürünlerin özellikleri başarıyla getirildi",
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new GetBrandFiltersResponse()
                {
                    HassError = true,
                    Message = "Marka'ya göre listelenen ürünlerin özellikleri  getirilemedi Hata alındı",
                    ErrorMessage = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString()
                };
            }

        }

        private async Task<List<Guid>> CollectProductIdsFromCategory(Guid brandId, CancellationToken cancellationToken)
        {
            var allProductIds = new List<Guid>();
            var stack = new Stack<Guid>();
            stack.Push(brandId);

            while (stack.Count > 0)
            {
                var currentCategoryId = stack.Pop();
                var category = await _brandReadRepository.GetWhere(c => c.ID == currentCategoryId)
                    .Select(c => new
                    {
                        ProductIds = c.Products.Select(p => p.ID),
                        SubCategoryIds = c.BrandAttributes.Select(sc => sc.ID)
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (category != null)
                {
                    allProductIds.AddRange(category.ProductIds);
                    foreach (var subCategoryId in category.SubCategoryIds)
                    {
                        stack.Push(subCategoryId);
                    }
                }
            }

            return allProductIds;
        }
    }
}
