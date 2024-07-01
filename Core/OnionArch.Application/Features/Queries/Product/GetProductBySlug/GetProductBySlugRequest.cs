using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Product.GetProductBySlug
{
    public class GetProductBySlugRequest:IRequest<GetProductBySlugResponse>
    {
        public string? Id { get; set; }
        public string? MaterializedProductPath { get; set; }
        public string? MaterializedProductPathByName { get; set; }
        public string? MaterializedProductPathBySlug { get; set; }
    }
}
