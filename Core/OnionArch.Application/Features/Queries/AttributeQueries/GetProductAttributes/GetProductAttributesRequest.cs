using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Attribute.GetProductAttributes
{
    public class GetProductAttributesRequest:IRequest<GetProductAttributesResponse>
    {
        public Guid productId { get; set; }
    }
}
