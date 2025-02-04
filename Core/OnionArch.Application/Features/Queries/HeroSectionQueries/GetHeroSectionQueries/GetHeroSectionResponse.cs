using OnionArch.Application.GlobalResponse;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.HeroSectionQueries.GetHeroSectionQueries
{
    public class GetHeroSectionResponse : GlobalResponseResult
    {
        public List<HeroSectionSlider> HeroSectionItems { get; set; }
    }
}
