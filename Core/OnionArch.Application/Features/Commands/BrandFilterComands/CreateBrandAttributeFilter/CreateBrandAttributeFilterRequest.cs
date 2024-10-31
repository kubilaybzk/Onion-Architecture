using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.BrandFilterComands.CreateBrandAttributeFilter
{
    public class CreateBrandAttributeFilterRequest:IRequest<CreateBrandAttributeFilterResponse>
    {
        public string JsonResult { get; set; }
        public string BrandSlug { get; set; }

        public int Order { get; set; } = 0;
    }
}
