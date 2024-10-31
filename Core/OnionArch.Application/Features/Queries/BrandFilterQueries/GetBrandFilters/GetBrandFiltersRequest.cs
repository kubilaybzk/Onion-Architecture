using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.BrandFilterQueries.GetBrandFilters
{
    public class GetBrandFiltersRequest:IRequest<GetBrandFiltersResponse>
    {
        public string BrandSlug { get; set; }
    }
}
