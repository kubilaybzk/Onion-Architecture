using MediatR;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Repositories.HeroSectionCruds.HeroSectionSliderCrud;
using OnionArch.Domain.Entities;
using System.Net;

namespace OnionArch.Application.Features.Commands.HeroSectionComands.CreateHeroSectionComands
{
    public class CreateHeroSectionHandler : IRequestHandler<CreateHeroSectionRequest, CreateHeroSectionResponse>
    {
        private readonly IStorageService _storageService;
        private readonly IHeroSectionImageWriteRepository _heroSectionImageWriteRepository;
        private readonly IHeroSectionSliderWriteRepository _heroSectionSliderWriteRepository;

        public CreateHeroSectionHandler(
            IStorageService storageService,
            IHeroSectionImageWriteRepository heroSectionImageWriteRepository,
            IHeroSectionSliderWriteRepository heroSectionSliderWriteRepository)
        {
            _storageService = storageService;
            _heroSectionImageWriteRepository = heroSectionImageWriteRepository;
            _heroSectionSliderWriteRepository = heroSectionSliderWriteRepository;
        }

        public async Task<CreateHeroSectionResponse> Handle(CreateHeroSectionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _storageService.UploadAsync("heroSection-images", request.HeroSectionImage);

                var heroSectionSlider = new HeroSectionSlider
                {
                    ImageAltTile = request.ImageAltTile,
                    ImageRederictLink = request.ImageRederictLink,
                    ImageRedirectLinkTitle = request.ImageRedirectLinkTitle,
                    Order = request.Order,
                    HtmlContent = request.HtmlContent,
                    isSliderImage = request.isSliderImage,
                    HeroSectionImages = result.Select((d, index) => new HeroSectionImage
                    {
                        FileName = d.fileName,
                        Path = d.PathOrContainerName,
                        Storage = _storageService.StorageType,
                        isSliderImage = request.isSliderImage,
                    }).ToList()
                };

                await _heroSectionSliderWriteRepository.AddAsync(heroSectionSlider);
                await _heroSectionSliderWriteRepository.SaveAsync();

                return new CreateHeroSectionResponse
                {
                    isCreated = true,
                    ErrorMessage=null,
                    HassError=false,
                    Message="Görsel Başarıyla eklendi",
                    StatusCode=HttpStatusCode.Created,
                    StatusCodeString=HttpStatusCode.Created.ToString()
                };
            }
            catch (Exception ex)
            {
                return new CreateHeroSectionResponse
                {
                    isCreated = false,
                    ErrorMessage = ex.Message,
                    HassError =true,
                    Message = "Görsel eklnmesi sırasında hata alındı.",
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString()
                };
            }
        }
    }
}