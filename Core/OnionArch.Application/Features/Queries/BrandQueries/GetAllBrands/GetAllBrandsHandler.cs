using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Repositories.BrandCrud;
using OnionArch.Application.View_Models.Brands;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.BrandQueries.GetAllBrands
{
    public class GetAllBrandsHandler : IRequestHandler <GetAllBrandsRequest, GetAllBrandsResponse>
    {
        private readonly IBrandReadRepository _brandReadRepository;

        public GetAllBrandsHandler(IBrandReadRepository brandReadRepository)
        {
            _brandReadRepository = brandReadRepository;
        }

        public async Task<GetAllBrandsResponse> Handle(GetAllBrandsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var brandList = await _brandReadRepository.Table
                      .AsNoTracking()
                      .Select(currentBrand => new VM_Brand_Result
                      {
                          Id=currentBrand.ID,
                          BrandName = currentBrand.BrandName,
                          BrandSlug = currentBrand.BrandSlug,
                          DetailTitle = currentBrand.DetailTitle,
                          DetailDescription = currentBrand.DetailDescription,
                          isActive = currentBrand.isActive,
                          SeoLinkTitle = currentBrand.SeoLinkTitle,
                          SeoLinkDescription = currentBrand.SeoLinkDescription,
                          SeoDetailTitle = currentBrand.DetailTitle,
                          SeoDetailDescription = currentBrand.DetailDescription,
                          BrandLogo = currentBrand.BrandLogo == null ? null : new VM_BrandImageFile_Result
                          {
                              FileName = currentBrand.BrandLogo.FileName,
                              Path = currentBrand.BrandLogo.Path,
                              Showcase = currentBrand.BrandLogo.Showcase,
                              Storage = currentBrand.BrandLogo.Storage,
                              SeoImageAltInformation= currentBrand.BrandLogo.SeoImageAltInformation
                          }
                      })
                      .OrderBy(p=>p.BrandName)
                      .ToListAsync(cancellationToken);
                return new GetAllBrandsResponse()
                {
                    BrandList = brandList,
                    ErrorMessage = "",
                    HassError = false,
                    Message = "Markalar başarıyla listelendi",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString(),
                };
            }
            catch (Exception ex)
            {
                return new GetAllBrandsResponse()
                {
                    BrandList =  null,
                    ErrorMessage =ex.Message,
                    HassError = false,
                    Message = "Markalar listelenirken hata alındı",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                };
            }
        }
    }
}
