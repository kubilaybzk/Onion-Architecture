using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Attribute.GetAllAttributes
{
    public class GetAllAttributesResponse : GlobalResponseResult
    {
        public List<Domain.Entities.Attribute> Attributes { get; set; }
    }
}
