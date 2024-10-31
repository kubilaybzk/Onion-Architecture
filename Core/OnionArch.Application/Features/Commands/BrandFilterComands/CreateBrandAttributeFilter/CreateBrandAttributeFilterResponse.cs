using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.BrandFilterComands.CreateBrandAttributeFilter
{
    public class CreateBrandAttributeFilterResponse: GlobalResponseResult
    {
        public Boolean isCreated { get; set; }
    }
}
