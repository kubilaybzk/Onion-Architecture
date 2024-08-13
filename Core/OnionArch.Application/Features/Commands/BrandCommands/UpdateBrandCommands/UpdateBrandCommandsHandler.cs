using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Repositories.BrandCrud;
using OnionArch.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.BrandCommands.UpdateBrandCommands
{
    public class UpdateBrandCommandsHandler : IRequestHandler<UpdateBrandCommandsRequest, UpdateBrandCommandsResponse>
    {
        private readonly IBrandReadRepository _brandReadRepository;
        private readonly IBrandWriteRepository _brandWriteRepository;
        private readonly IStorageService _storageService;

        public UpdateBrandCommandsHandler(IBrandReadRepository brandReadRepository, IBrandWriteRepository brandWriteRepository, IStorageService storageService)
        {
            _brandReadRepository = brandReadRepository;
            _brandWriteRepository = brandWriteRepository;
            _storageService = storageService;
        }

        public async Task<UpdateBrandCommandsResponse> Handle(UpdateBrandCommandsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var currentBrand =  _brandReadRepository.Table.Include(p=>p.BrandLogo).Where(p=>p.ID==Guid.Parse(request.Id)).FirstOrDefault();
                if (currentBrand == null)
                    return CreateResponse(false, "Marka bulunamadı","", System.Net.HttpStatusCode.NotFound);

                UpdateBrandProperties(currentBrand, request);
                await UpdateBrandLogo(currentBrand, request);

                await _brandWriteRepository.SaveAsync();

                return CreateResponse(true, "Marka güncelleme başarılı","", System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CreateResponse(false, "Marka güncelleme başarısız",ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        private void UpdateBrandProperties(Brand brand, UpdateBrandCommandsRequest request)
        {
            brand.BrandName = request.BrandName;
            brand.BrandSlug = request.BrandSlug;
            brand.DetailTitle = request.DetailTitle;
            brand.DetailDescription = request.DetailDescription;
            brand.isActive = request.isActive;
            brand.SeoLinkTitle = request.SeoLinkTitle;
            brand.SeoLinkDescription = request.SeoLinkDescription;
            brand.SeoDetailTitle = request.DetailTitle;
            brand.SeoDetailDescription = request.DetailDescription;

            if (brand.BrandLogo != null)
            {
                brand.BrandLogo.SeoImageAltInformation = request.DetailDescription;
            }
        }

        private async Task UpdateBrandLogo(Brand brand, UpdateBrandCommandsRequest request)
        {
            if (request.BrandLogo != null)
            {
                if (brand.BrandLogo != null)
                {
                    await _storageService.DeleteFileAsync(brand.BrandLogo.FileName, "wwwroot/resource/brand-images");
                    // Mevcut logo dosyasını veritabanından da sil
                    brand.BrandLogo = null;
                    await _brandWriteRepository.SaveAsync();
                }

                var result = await _storageService.UploadAsync("brand-images", request.BrandLogo);
                brand.BrandLogo = result.Select(p => new BrandImageFile
                {
                    FileName = p.fileName ?? string.Empty,
                    Storage = p.PathOrContainerName ?? string.Empty,
                    Path = p.PathOrContainerName ?? string.Empty,
                    SeoImageAltInformation = request.SeoImageAltInformation,
                }).FirstOrDefault();
            }
        }

        private UpdateBrandCommandsResponse CreateResponse(bool isSuccess, string message, string Errormessage, System.Net.HttpStatusCode statusCode)
        {
            return new UpdateBrandCommandsResponse
            {
                ErrorMessage = isSuccess ? string.Empty : Errormessage,
                HassError = !isSuccess,
                isUpdated = isSuccess,
                Message = message,
                StatusCode = statusCode,
                StatusCodeString = statusCode.ToString()
            };
        }
    }
}