using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Repositories.BrandCrud;
using OnionArch.Application.View_Models.CategoryFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.BrandFilterQueries.GetAllBrandProductsAttributes
{
    public class GetAllBrandProductsAttributesHandler : IRequestHandler<GetAllBrandProductsAttributesRequest, GetAllBrandProductsAttributesResponse>
    {
        private readonly IBrandReadRepository _brandReadRepository;
        private readonly IProductReadRepository _productReadRepository;

        public GetAllBrandProductsAttributesHandler(IBrandReadRepository brandReadRepository, IProductReadRepository productReadRepository)
        {
            _brandReadRepository = brandReadRepository;
            _productReadRepository = productReadRepository;
        }
        public async Task<GetAllBrandProductsAttributesResponse> Handle(GetAllBrandProductsAttributesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _brandReadRepository.GetWhere(c => c.BrandSlug == request.BrandSlug)
                    .Select(c => new
                    {
                        c.ID,
                        BrandAttributes = c.BrandAttributes.Select(ca => new
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
                    return new GetAllBrandProductsAttributesResponse
                    {
                        HassError = true,
                        Message = "Kategori bulunamadı",
                        StatusCode = System.Net.HttpStatusCode.NotFound
                    };
                }

                // Önce ProductId'leri bir listeye alalım
                var productIds = await _brandReadRepository.GetWhere(c => c.ID == category.ID)
                    .SelectMany(c => c.Products.Select(p => p.ID))
                    .ToListAsync(cancellationToken);

                var attributes = await _productReadRepository.GetWhere(p => productIds.Contains(p.ID))
                 .SelectMany(p => p.ProductAttributes)
                 .Select(pa => new
                 {
                     AttributeId = pa.AttributeValue.Attribute.ID,
                     AttributeName = pa.AttributeValue.Attribute.Name,
                     AttributeValueId = pa.AttributeValue.ID,
                     AttributeValue = pa.AttributeValue.Value,
                     AttributeValueSlug = pa.AttributeValue.ValueForSlug,
                     AttributeSlug = pa.AttributeValue.Attribute.NameForSlug
                 })
                 .Distinct()
                 .ToListAsync(cancellationToken);

                 

                var uniqueAttributes = attributes
                    .GroupBy(a => new { a.AttributeId, a.AttributeName, a.AttributeSlug })
                    .Select(p => new VM_Category_Filters_Result()
                    {
                        FilterType = null,
                        AttributeId = p.Key.AttributeId.ToString(),
                        AttributeName = p.Key.AttributeName,
                        AttributeNameSlug = p.Key.AttributeSlug,
                        AttributeValues = p.Select(a => new VM_Category_FilterValue_Result()
                        {
                            AttributeValue = a.AttributeValue,
                            AttributeValueSlug = a.AttributeValueSlug,
                            AttributeValueId = a.AttributeValueId.ToString(),

                        }).ToList()
                    })
                    .OrderBy(a => a.AttributeName)
                    .ToList();

                return new GetAllBrandProductsAttributesResponse
                {
                    BrandFilters = uniqueAttributes,
                    Message = "Ürün özellikleri başarıyla getirildi",
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new GetAllBrandProductsAttributesResponse()
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
