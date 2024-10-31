using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.ProductAttributesQueries.GetCategoriesFilter
{
    public class GetCategoriesFilterRequest:IRequest<GetCategoriesFilterResponse>
    {
        public string CategorySlug {  get; set; }
    }
}
