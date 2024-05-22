using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Product.GetProductByCategory
{
    public class GetProductByCategoryRequest :IRequest<GetProductByCategoryResponse>
    {
        public string? CategorySlug { get; set; }
        public string? Id { get; set; }
        public string? CategoryName { get; set; }
    }
}
