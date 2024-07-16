using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Features.Queries.ProductAttributesQueries.GetAllProductAttributesWithOutFilter;
using OnionArch.Application.Repositories.AttributeCrud.CategoryAttributeCrud;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Application.View_Models.CategoryFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.ProductAttributesQueries.GetCategoriesFilter
{
    public class GetCategoriesFilterHandler : IRequestHandler<GetCategoriesFilterRequest, GetCategoriesFilterResponse>
    {
 
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly IProductReadRepository _productReadRepository;

        public GetCategoriesFilterHandler(ICategoryReadRepository categoryReadRepository, IProductReadRepository productReadRepository)
        {
 
            _categoryReadRepository = categoryReadRepository;
            _productReadRepository = productReadRepository;
        }

        public async Task<GetCategoriesFilterResponse> Handle(GetCategoriesFilterRequest request, CancellationToken cancellationToken)
        {


            try
            {
                var category = await _categoryReadRepository.GetWhere(c => c.CategorySlug == request.CategorySlug)
                    .Select(c => new
                    {
                        c.ID,
                        CategoryAttributes = c.CategoryAttributes.Select(ca => new
                        {
                            ca.FilterType,
                            ca.FilterId,
                            ca.Order,
                            FilterName = ca.Filter.Name,
                            FilterNameForSlug = ca.Filter.NameForSlug
                        }).OrderBy(ca => ca.Order).ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (category == null)
                {
                    return new GetCategoriesFilterResponse
                    {
                        HassError = true,
                        Message = "Kategori bulunamadı",
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

                return new GetCategoriesFilterResponse
                {
                    CategoryFilters = categoryFilters,
                    Message = "Ürün özellikleri başarıyla getirildi",
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new GetCategoriesFilterResponse()
                {
                    HassError = true,
                    Message = "Hata alındı",
                    ErrorMessage = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString()
                };
            }
        }

        private async Task<List<Guid>> CollectProductIdsFromCategory(Guid categoryId, CancellationToken cancellationToken)
        {
            var allProductIds = new List<Guid>();
            var stack = new Stack<Guid>();
            stack.Push(categoryId);

            while (stack.Count > 0)
            {
                var currentCategoryId = stack.Pop();
                var category = await _categoryReadRepository.GetWhere(c => c.ID == currentCategoryId)
                    .Select(c => new
                    {
                        ProductIds = c.Products.Select(p => p.ID),
                        SubCategoryIds = c.SubCategories.Select(sc => sc.ID)
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