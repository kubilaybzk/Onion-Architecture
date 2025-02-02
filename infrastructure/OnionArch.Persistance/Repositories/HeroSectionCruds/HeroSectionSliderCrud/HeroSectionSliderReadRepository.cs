using OnionArch.Application.Repositories.HeroSectionCruds.HeroSectionSliderCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Repositories.HeroSectionCruds.HeroSectionSliderCrud
{
    public class HeroSectionSliderReadRepository : ReadRepository<HeroSectionSlider>, IHeroSectionSliderReadRepository
    {
        public HeroSectionSliderReadRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
