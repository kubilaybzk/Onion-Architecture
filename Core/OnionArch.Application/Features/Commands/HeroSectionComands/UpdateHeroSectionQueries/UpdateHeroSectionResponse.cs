using OnionArch.Application.GlobalResponse;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.HeroSectionComands.UpdateHeroSectionQueries
{
    public class UpdateHeroSectionResponse : GlobalResponseResult
    {
        public bool isUpdated { get; set; }
    }
}
