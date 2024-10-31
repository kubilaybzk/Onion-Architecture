using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.BrandFilterQueries.GetAllBrandProductsAttributes
{
    public class GetAllBrandProductsAttributesRequest: IRequest<GetAllBrandProductsAttributesResponse>
    {
        public string BrandSlug { get; set; }
    }
}
