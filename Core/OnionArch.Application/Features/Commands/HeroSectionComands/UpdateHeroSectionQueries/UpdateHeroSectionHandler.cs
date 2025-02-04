using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Repositories.HeroSectionCruds.HeroSectionSliderCrud;
using OnionArch.Domain.Entities;
using System.Net;

namespace OnionArch.Application.Features.Commands.HeroSectionComands.UpdateHeroSectionQueries
{
    public class UpdateHeroSectionHandler : IRequestHandler<UpdateHeroSectionRequest, UpdateHeroSectionResponse>
    {
        private readonly IHeroSectionSliderReadRepository  _heroSectionSliderReadRepository;
        private readonly IHeroSectionSliderWriteRepository _heroSectionSliderWriteRepository;
        private readonly IStorageService _storageService;
        private bool _disposed;

        public UpdateHeroSectionHandler(
            IHeroSectionSliderReadRepository heroSectionSliderReadRepository,
            IHeroSectionSliderWriteRepository heroSectionSliderWriteRepository,
            IStorageService storageService)
        {
            _heroSectionSliderReadRepository = heroSectionSliderReadRepository;
            _heroSectionSliderWriteRepository = heroSectionSliderWriteRepository;
            _storageService = storageService;
        }

        public async Task<UpdateHeroSectionResponse> Handle(UpdateHeroSectionRequest request, CancellationToken cancellationToken)
        {
       

        
            try
            {
                // Input validasyonu
                if (string.IsNullOrEmpty(request.HeroItemID))
                    throw new ArgumentNullException(nameof(request.HeroItemID), "Id değeri gerekli");

                // Veriyi çekme ve varlik kontrolü
                var heroSection = await _heroSectionSliderReadRepository
                    .GetWhere(p => p.ID == Guid.Parse(request.HeroItemID))
                    .FirstOrDefaultAsync(cancellationToken);

                if (heroSection == null)
                    throw new KeyNotFoundException($"ID: {request.HeroItemID} olan hero section bulunamadı");

               

                // Diğer alanları güncelleme
                heroSection.ImageAltTile = request.ImageAltTile ?? heroSection.ImageAltTile;
                heroSection.ImageRederictLink = request.ImageRederictLink ?? heroSection.ImageRederictLink;
                heroSection.ImageRedirectLinkTitle = request.ImageRedirectLinkTitle ?? heroSection.ImageRedirectLinkTitle;
                heroSection.Order = request.Order > 0 ? request.Order : heroSection.Order;
                heroSection.HtmlContent = request.HtmlContent;
                heroSection.isSliderImage = request.isSliderImage;

                // Değişiklikleri kaydetme
                await _heroSectionSliderWriteRepository.SaveAsync();

                return new UpdateHeroSectionResponse
                {
                    isUpdated = true,
                    Message = "Güncelleme işlemi başarılı",
                    HassError = false,
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString()
                };
            }
            catch (ArgumentNullException ex)
            {
                return new UpdateHeroSectionResponse
                {
                    isUpdated = false,
                    ErrorMessage = ex.Message,
                    Message = "Geçersiz ID değeri",
                    HassError = true,
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusCodeString = HttpStatusCode.BadRequest.ToString()
                };
            }
            catch (KeyNotFoundException ex)
            {
                return new UpdateHeroSectionResponse
                {
                    isUpdated = false,
                    ErrorMessage = ex.Message,
                    Message = "Kayıt bulunamadı",
                    HassError = true,
                    StatusCode = HttpStatusCode.NotFound,
                    StatusCodeString = HttpStatusCode.NotFound.ToString()
                };
            }
            catch (Exception ex)
            {
                return new UpdateHeroSectionResponse
                {
                    isUpdated = false,
                    ErrorMessage = ex.Message,
                    Message = "Güncelleme işlemi başarısız",
                    HassError = true,
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString()
                };
            }
       
    }
    }
}