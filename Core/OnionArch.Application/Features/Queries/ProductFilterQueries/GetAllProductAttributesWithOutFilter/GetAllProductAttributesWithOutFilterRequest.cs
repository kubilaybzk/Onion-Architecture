using MediatR;
using OnionArch.Application.CQRS_Globals.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.ProductAttributesQueries.GetAllProductAttributesWithOutFilter
{
    public class GetAllProductAttributesWithOutFilterRequest : IRequest<GetAllProductAttributesWithOutFilterResponse>
    {
        public string CategorySlug { get; set; }
        public GlobalPaginationRequest PaginationValues { get; set; }
    }
}
