using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Features.Queries.BrandQueries.GetAllBrands;
using OnionArch.Application.Features.Queries.BrandQueries.GetBrandNameWithIdQueries;
using OnionArch.Application.Repositories.BrandCrud;
using OnionArch.Application.View_Models.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.BrandQueries.GetAllBrandNameQueries
{
    public class GetBrandNameWithIdHandler : IRequestHandler<GetAllBrandNameRequest, GetAllBrandNameResponse>
    {
        private readonly IBrandReadRepository _brandReadRepository;

        public GetBrandNameWithIdHandler(IBrandReadRepository brandReadRepository)
        {
            _brandReadRepository = brandReadRepository;
        }

        public  async Task<GetAllBrandNameResponse> Handle(GetAllBrandNameRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var brandList = await _brandReadRepository.GetAll()
                      .Select(currentBrand => new VM_BrandNameWithId_Result
                      {
                          Id = currentBrand.ID,
                          BrandName = currentBrand.BrandName,
                          BrandSlug = currentBrand.BrandSlug,                          
                      })
                      .OrderBy(p => p.BrandName)
                      .ToListAsync(cancellationToken);
                return new GetAllBrandNameResponse()
                {
                    BrandIdsWithName = brandList,
                    ErrorMessage = "",
                    HassError = false,
                    Message = "Markalar başarıyla listelendi",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString(),
                };
            }
            catch (Exception ex)
            {
                return new GetAllBrandNameResponse()
                {
                    BrandIdsWithName = null,
                    ErrorMessage = ex.Message,
                    HassError = false,
                    Message = "Markalar listelenirken hata alındı",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                };
            }
        }
    }
}
