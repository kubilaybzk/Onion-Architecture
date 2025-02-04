using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Features.Commands.HeroSectionComands.CreateHeroSectionComands;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.HeroSectionComands.UpdateHeroSectionQueries
{
    public class UpdateHeroSectionRequest : IRequest<UpdateHeroSectionResponse>
    {
        public string HeroItemID { get; set; }
        public string ImageAltTile { get; set; }
        public string ImageRederictLink { get; set; }
        public string ImageRedirectLinkTitle { get; set; }
        public int Order { get; set; }
        public bool isSliderImage { get; set; }
        public string? HtmlContent { get; set; }
    }
}
