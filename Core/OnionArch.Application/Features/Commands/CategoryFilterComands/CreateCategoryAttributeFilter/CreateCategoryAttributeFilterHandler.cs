using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Repositories.AttributeCrud.AttributeCrud;
using OnionArch.Application.Repositories.AttributeCrud.CategoryAttributeCrud;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.ProductFilterComands.SaveCategoryAttributeFilter
{
    public class CreateCategoryAttributeFilterHandler : IRequestHandler<CreateCategoryAttributeFilterRequest, CreateCategoryAttributeFilterResponse>
    {
        private readonly ICategoryAttributeReadRepository _categoryAttributeReadRepository;
        private readonly ICategoryAttributeWriteRepository _categoryAttributeWriteRepository;
        private readonly IAttributeReadRepository _attributeReadRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly ICategoryWriteRepository _categoryWriteRepository;

        public CreateCategoryAttributeFilterHandler(ICategoryAttributeReadRepository categoryAttributeReadRepository, ICategoryAttributeWriteRepository categoryAttributeWriteRepository, IAttributeReadRepository attributeReadRepository, ICategoryReadRepository categoryReadRepository, ICategoryWriteRepository categoryWriteRepository)
        {
            _categoryAttributeReadRepository = categoryAttributeReadRepository;
            _categoryAttributeWriteRepository = categoryAttributeWriteRepository;
            _attributeReadRepository = attributeReadRepository;
            _categoryReadRepository = categoryReadRepository;
            _categoryWriteRepository = categoryWriteRepository;
        }


        public async Task<CreateCategoryAttributeFilterResponse> Handle(CreateCategoryAttributeFilterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var targetCategory = await _categoryReadRepository.Table
                    .Include(p => p.CategoryAttributes)
                    .Where(p => p.CategorySlug == request.CategorySlug)
                    .SingleOrDefaultAsync();

                if (targetCategory == null)
                {
                    return new CreateCategoryAttributeFilterResponse()
                    {
                        Message = "Kategori bulunamadı",
                        StatusCode = HttpStatusCode.NotFound,
                        StatusCodeString = HttpStatusCode.NotFound.ToString(),
                        HassError = true,
                        ErrorMessage = "",
                        isCreated = false
                    };
                }

                var targetCategoryFilterResultsConvertedJson = System.Text.Json.JsonSerializer.Deserialize<List<AttributeJsonConvert>>(request.JsonResult);
                var existingCategoryFilters = targetCategory.CategoryAttributes.ToDictionary(cf => cf.FilterId);

                var categoryAttributesToAdd = new List<CategoryAttribute>();
                var categoryAttributesToUpdate = new List<CategoryAttribute>();
                var categoryAttributesToDelete = new List<CategoryAttribute>();

                foreach (var eachCategoryFilter in targetCategoryFilterResultsConvertedJson)
                {
                    var attribute = await _attributeReadRepository.GetByIdAsync(eachCategoryFilter.Id.ToString());

                    if (!existingCategoryFilters.TryGetValue(attribute.ID, out var existingFilter))
                    {
                        categoryAttributesToAdd.Add(new CategoryAttribute
                        {
                            Category = targetCategory,
                            Filter = attribute,
                            FilterType = eachCategoryFilter.FilterType,
                            Order = eachCategoryFilter.Order,
                        });
                    }
                    else
                    {
                        if (existingFilter.FilterType != eachCategoryFilter.FilterType)
                        {
                            existingFilter.FilterType = eachCategoryFilter.FilterType;
                            existingFilter.Order = eachCategoryFilter.Order;
                            categoryAttributesToUpdate.Add(existingFilter);
                        }
                        existingCategoryFilters.Remove(attribute.ID);
                    }
                }

                // Kalan mevcut filtreler silinecek
                categoryAttributesToDelete.AddRange(existingCategoryFilters.Values);

                // Bulk insert for new records
                if (categoryAttributesToAdd.Any())
                {
                    await _categoryAttributeWriteRepository.AddRangeAsync(categoryAttributesToAdd);
                }

                // Bulk update for existing records
                if (categoryAttributesToUpdate.Any())
                {
                    foreach (var updatedList in categoryAttributesToUpdate)
                    {
                        _categoryAttributeWriteRepository.Update(updatedList);
                    }
                }

                // Bulk delete for removed records
                if (categoryAttributesToDelete.Any())
                {
                    _categoryAttributeWriteRepository.RemoveRange(categoryAttributesToDelete);
                }

                await _categoryAttributeWriteRepository.SaveAsync();

                return new CreateCategoryAttributeFilterResponse()
                {
                    Message = "Filtreler başarıyla kaydedildi",
                    StatusCode = HttpStatusCode.Created,
                    StatusCodeString = HttpStatusCode.Created.ToString(),
                    HassError = false,
                    ErrorMessage = "",
                    isCreated = true
                };
            }
            catch (Exception ex)
            {
                return new CreateCategoryAttributeFilterResponse()
                {
                    Message = "Filtreler kaydedilirken hata",
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                    HassError = true,
                    ErrorMessage = ex.Message,
                    isCreated = false
                };
            }

        }
    }

    public class AttributeJsonConvert
    {
        [JsonPropertyName("attributeName")]
        public string AttributeName { get; set; }

        [JsonPropertyName("filterType")]
        public string FilterType { get; set; }

        [JsonPropertyName("attributeId")]
        public Guid Id { get; set; }

        [JsonPropertyName("attributeNameSlug")]
        public string AttributeNameSlug { get; set; }

        [JsonPropertyName("attributeValues")]
        public AttributeValueJsonConvert[] AttributeValues { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }
    }

    public partial class AttributeValueJsonConvert
    {
        [JsonPropertyName("attributeValueId")]
        public Guid AttributeValueId { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("attributeValueSlug")]
        public string AttributeValueSlug { get; set; }
    }
}
