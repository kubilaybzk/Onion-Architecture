using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.HeroSectionComands.CreateHeroSectionComands
{
    public class CreateHeroSectionResponse:GlobalResponseResult
    {
        public bool isCreated { get; set; }
    }
}
