using OnionArch.Application.GlobalResponse;
using OnionArch.Application.View_Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Product.GetProductBySlug
{
    public class GetProductBySlugResponse:GlobalResponseResult
    {
        public VM_Result_ProductLink Product { get; set; }
    }
}
