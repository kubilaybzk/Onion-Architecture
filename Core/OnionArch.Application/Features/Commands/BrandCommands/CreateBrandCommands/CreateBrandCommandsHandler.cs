using Common.CommonOperations;
using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.CategoryServices;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Repositories.BrandCrud;
using OnionArch.Application.Repositories.BrandImageFileCrud;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.BrandCommands.CreateBrandCommands
{
    public class CreateBrandCommandsHandler : IRequestHandler<CreateBrandCommandsRequest, CreateBrandCommandsResponse>
    {

        private readonly IBrandWriteRepository _brandWriteRepository;
        private readonly IBrandImageFileWriteRepository _brandImageFileWriteRepository;
        private readonly IStorageService _storageService;
        public CreateBrandCommandsHandler(IBrandWriteRepository brandWriteRepository, IBrandImageFileWriteRepository brandImageFileWriteRepository, IStorageService storageService)
        {
            _brandWriteRepository = brandWriteRepository;
            _brandImageFileWriteRepository = brandImageFileWriteRepository;
            _storageService = storageService;
        }



        public async Task<CreateBrandCommandsResponse> Handle(CreateBrandCommandsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var newBrand = new Brand()
                {
                    BrandName = request.BrandName,
                    DetailTitle = request.DetailTitle,
                    DetailDescription = request.DetailDescription,
                    isActive = request.isActive,
                    SeoLinkTitle = request.SeoLinkTitle,
                    SeoLinkDescription = request.SeoLinkDescription,
                    SeoDetailTitle = request.DetailTitle,
                    SeoDetailDescription = request.DetailDescription,
                };

                newBrand.BrandSlug = StringUtilities.GenerateSlug(request.BrandName);

                if (request.BrandLogo != null)
                {
                    var result = await _storageService.UploadAsync("brand-images", request.BrandLogo);
                    newBrand.BrandLogo = result.Select(p => new BrandImageFile()
                    {
                        FileName = p.fileName ?? string.Empty,
                        Storage = p.PathOrContainerName ?? string.Empty,
                        Path = p.PathOrContainerName ?? string.Empty,
                        SeoImageAltInformation = request.SeoImageAltInformation,
                    }).FirstOrDefault();
                }

                await _brandWriteRepository.AddAsync(newBrand);
                await _brandWriteRepository.SaveAsync();

                return new CreateBrandCommandsResponse()
                {
                    ErrorMessage = "",
                    isCreated = true,
                    Message = "Marka başarıyla oluşturuldu",
                    HassError = false,
                    StatusCode = System.Net.HttpStatusCode.Created,
                    StatusCodeString = System.Net.HttpStatusCode.Created.ToString()
                };
            }
            catch (Exception ex)
            {
                return new CreateBrandCommandsResponse()
                {
                    ErrorMessage = ex.Message,
                    isCreated = false,
                    Message = "Marka oluşturulurken bir hata alındı",
                    HassError = false,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString()
                };

            }

        }
    }
}
