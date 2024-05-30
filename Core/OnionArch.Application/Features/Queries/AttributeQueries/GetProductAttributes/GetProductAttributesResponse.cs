using OnionArch.Application.GlobalResponse;
using OnionArch.Application.View_Models.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Attribute.GetProductAttributes
{
    public class GetProductAttributesResponse:GlobalResponseResult
    {
        public List<VM_Product_Attributes> ProductAttributes { get; set; }
    }
   
}
