using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Features.Commands.HeroSectionComands.CreateHeroSectionComands;
using OnionArch.Application.Repositories.HeroSectionCruds.HeroSectionSliderCrud;
using OnionArch.Domain.Entities;
using System.Net;

namespace OnionArch.Application.Features.Queries.HeroSectionQueries.GetHeroSectionQueries
{
    public class GetHeroSectionHandler : IRequestHandler<GetHeroSectionRequest, GetHeroSectionResponse>
    {
        private readonly IHeroSectionSliderReadRepository _heroSectionSliderReadRepository;

        public GetHeroSectionHandler(IHeroSectionSliderReadRepository heroSectionSliderReadRepository)
        {
            _heroSectionSliderReadRepository = heroSectionSliderReadRepository;
        }

        public async Task<GetHeroSectionResponse> Handle(GetHeroSectionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var heroSectionImages = _heroSectionSliderReadRepository.GetAll().Include(p=>p.HeroSectionImages).Select(item => new HeroSectionSlider()
                {
                    Order = item.Order,
                    isDeleted = item.isDeleted,
                    ImageAltTile = item.ImageAltTile,
                    ImageRederictLink = item.ImageRederictLink,
                    ImageRedirectLinkTitle = item.ImageRedirectLinkTitle,
                    ID=item.ID,
                    HeroSectionImages = item.HeroSectionImages.Select(pif => new HeroSectionImage()
                    {
                        FileName = pif.FileName,
                        Path = pif.Path,
                        Storage = pif.Storage,
                        isSliderImage = pif.isSliderImage,
                        isDeleted = pif.isDeleted,
                        ID=pif.ID

                    }).ToList(),

                });
                return new GetHeroSectionResponse()
                {
                    ErrorMessage = null,
                    HassError = false,
                    HeroSectionItems = heroSectionImages.ToList(),
                    Message = "Banner alanı Başarıyla Listelendi",
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString()

                };
            }
            catch(Exception e)
            {
                return new GetHeroSectionResponse()
                {
                    ErrorMessage = e.Message,
                    HassError = true,
                    HeroSectionItems = null,
                    Message = "Banner alanı listelenirken hata alındı.",
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString()

                };
            }
           
        }
    }
}