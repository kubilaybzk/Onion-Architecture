using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Features.Commands.ProductFilterComands.SaveCategoryAttributeFilter;
using OnionArch.Application.Repositories.AttributeCrud.AttributeCrud;
using OnionArch.Application.Repositories.AttributeCrud.CategoryAttributeCrud;
using OnionArch.Application.Repositories.BrandAttributeCrud;
using OnionArch.Application.Repositories.BrandCrud;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.BrandFilterComands.CreateBrandAttributeFilter
{
    public class CreateBrandAttributeFilterHandler : IRequestHandler<CreateBrandAttributeFilterRequest, CreateBrandAttributeFilterResponse>
    {
        private readonly IBrandAttributeReadRepository _brandAttributeReadRepository;
        private readonly IBrandAttributeWriteRepository _brandAttributeWriteRepository;
        private readonly IBrandReadRepository _brandReadRepository;
        private readonly IBrandWriteRepository _brandWriteRepository;
        private readonly IAttributeReadRepository _attributeReadRepository;


        public CreateBrandAttributeFilterHandler(IAttributeReadRepository attributeReadRepository, IBrandAttributeWriteRepository brandAttributeWriteRepository, IBrandAttributeReadRepository brandAttributeReadRepository, IBrandReadRepository brandReadRepository, IBrandWriteRepository brandWriteRepository)
        {

            _attributeReadRepository = attributeReadRepository;
            _brandAttributeWriteRepository = brandAttributeWriteRepository;
            _brandAttributeReadRepository = brandAttributeReadRepository;
            _brandReadRepository = brandReadRepository;
            _brandWriteRepository = brandWriteRepository;
        }

        public async Task<CreateBrandAttributeFilterResponse> Handle(CreateBrandAttributeFilterRequest request, CancellationToken cancellationToken)
        {

            try
            {
                var targetBrand = await _brandReadRepository.Table
                    .Include(p => p.BrandAttributes)
                    .Where(p => p.BrandSlug == request.BrandSlug)
                    .SingleOrDefaultAsync();

                if (targetBrand == null)
                {
                    return new CreateBrandAttributeFilterResponse()
                    {
                        Message = "Kategori bulunamadı",
                        StatusCode = HttpStatusCode.NotFound,
                        StatusCodeString = HttpStatusCode.NotFound.ToString(),
                        HassError = true,
                        ErrorMessage = "",
                        isCreated = false
                    };
                }

                var targetBrandFilterResultsConvertedJson = System.Text.Json.JsonSerializer.Deserialize<List<AttributeJsonConvert>>(request.JsonResult);
                var existingCategoryFilters = targetBrand.BrandAttributes.ToDictionary(cf => cf.FilterId);

                var brandAttributeToAdd = new List<BrandAttribute>();
                var brandAttributeToUpdate = new List<BrandAttribute>();
                var brandAttributeToDelete = new List<BrandAttribute>();

                foreach (var eachCategoryFilter in targetBrandFilterResultsConvertedJson)
                {
                    var attribute = await _attributeReadRepository.GetByIdAsync(eachCategoryFilter.Id.ToString());

                    if (!existingCategoryFilters.TryGetValue(attribute.ID, out var existingFilter))
                    {
                        brandAttributeToAdd.Add(new BrandAttribute
                        {
                            Brand = targetBrand,
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
                            brandAttributeToUpdate.Add(existingFilter);
                        }
                        existingCategoryFilters.Remove(attribute.ID);
                    }
                }

                // Kalan mevcut filtreler silinecek
                brandAttributeToDelete.AddRange(existingCategoryFilters.Values);

                // Bulk insert for new records
                if (brandAttributeToAdd.Any())
                {
                    await _brandAttributeWriteRepository.AddRangeAsync(brandAttributeToAdd);
                }

                // Bulk update for existing records
                if (brandAttributeToUpdate.Any())
                {
                    foreach (var updatedList in brandAttributeToUpdate)
                    {
                        _brandAttributeWriteRepository.Update(updatedList);
                    }
                }

                // Bulk delete for removed records
                if (brandAttributeToDelete.Any())
                {
                    _brandAttributeWriteRepository.RemoveRange(brandAttributeToDelete);
                }

                await _brandAttributeWriteRepository.SaveAsync();

                return new CreateBrandAttributeFilterResponse()
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
                return new CreateBrandAttributeFilterResponse()
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
}
