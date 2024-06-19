using MediatR;
using OnionArch.Application.CQRS_Globals;
using OnionArch.Application.CQRS_Globals.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Product.GetLatesProducts
{
    public class GetLatesProductsRequest: GlobalPaginationRequest, IRequest<GetLatesProductsResponse>
    {   
        public string? ProductCodeOrProductName { get; set; }

    }
}
