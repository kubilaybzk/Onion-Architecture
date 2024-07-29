using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Product.GetAllProductSlugs
{
    public class GetAllProductSlugsRequest:IRequest<GetAllProductSlugsResponse>
    {
    }
}
