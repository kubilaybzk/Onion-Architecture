using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Features.Commands.HeroSectionComands.CreateHeroSectionComands;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.HeroSectionComands.DeleteHeroSectionQueries
{
    public class DeleteHeroSectionRequest : IRequest<DeleteHeroSectionResponse>
    {
        public string HeroItemID { get; set; }
    }
}
