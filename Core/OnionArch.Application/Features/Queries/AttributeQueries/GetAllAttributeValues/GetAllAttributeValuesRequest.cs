using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Attribute.GetAllAttributeValues
{
    public class GetAllAttributeValuesRequest : IRequest<GetAllAttributeValuesResponse>
    {
        public Guid AttributeId { get; set; }
    }
}
