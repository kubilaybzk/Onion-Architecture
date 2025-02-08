using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Repositories.HeroSectionCruds.HeroSectionSliderCrud;
using OnionArch.Domain.Entities;
using System.Net;

namespace OnionArch.Application.Features.Commands.HeroSectionComands.DeleteHeroSectionQueries
{
    public class DeleteHeroSectionHandler : IRequestHandler<DeleteHeroSectionRequest, DeleteHeroSectionResponse>
    {
        private readonly IHeroSectionSliderReadRepository _heroSectionSliderReadRepository;
        private readonly IHeroSectionSliderWriteRepository _heroSectionSliderWriteRepository;
        private readonly IStorageService _storageService;
        private bool _disposed;

        public DeleteHeroSectionHandler(
            IHeroSectionSliderReadRepository heroSectionSliderReadRepository,
            IHeroSectionSliderWriteRepository heroSectionSliderWriteRepository,
            IStorageService storageService)
        {
            _heroSectionSliderReadRepository = heroSectionSliderReadRepository;
            _heroSectionSliderWriteRepository = heroSectionSliderWriteRepository;
            _storageService = storageService;
        }

        public async Task<DeleteHeroSectionResponse> Handle(DeleteHeroSectionRequest request, CancellationToken cancellationToken)
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


                await _heroSectionSliderWriteRepository.RemoveAsync(heroSection.ID.ToString());
                await _heroSectionSliderWriteRepository.SaveAsync();

                return new DeleteHeroSectionResponse
                {
                    isDeleted = true,
                    Message = "Silme işlemi başarılı",
                    HassError = false,
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString()
                };
            }
            catch (ArgumentNullException ex)
            {
                return new DeleteHeroSectionResponse
                {
                    isDeleted = false,
                    ErrorMessage = ex.Message,
                    Message = "Geçersiz ID değeri",
                    HassError = true,
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusCodeString = HttpStatusCode.BadRequest.ToString()
                };
            }
            catch (KeyNotFoundException ex)
            {
                return new DeleteHeroSectionResponse
                {
                    isDeleted = false,
                    ErrorMessage = ex.Message,
                    Message = "Kayıt bulunamadı",
                    HassError = true,
                    StatusCode = HttpStatusCode.NotFound,
                    StatusCodeString = HttpStatusCode.NotFound.ToString()
                };
            }
            catch (Exception ex)
            {
                return new DeleteHeroSectionResponse
                {
                    isDeleted = false,
                    ErrorMessage = ex.Message,
                    Message = "Silme işlemi başarısız",
                    HassError = true,
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString()
                };
            }

        }
    }
}