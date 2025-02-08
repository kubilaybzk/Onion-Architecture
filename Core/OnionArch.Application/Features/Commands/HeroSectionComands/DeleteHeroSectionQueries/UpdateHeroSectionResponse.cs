using OnionArch.Application.GlobalResponse;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.HeroSectionComands.DeleteHeroSectionQueries
{
    public class DeleteHeroSectionResponse : GlobalResponseResult
    {
        public bool isDeleted { get; set; }
    }
}
