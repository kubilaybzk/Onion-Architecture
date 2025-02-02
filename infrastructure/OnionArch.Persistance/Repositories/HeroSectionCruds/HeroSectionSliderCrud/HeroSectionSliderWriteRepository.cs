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
    public class HeroSectionSliderWriteRepository : WriteRepository<HeroSectionSlider>, IHeroSectionSliderWriteRepository
    {
        public HeroSectionSliderWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
