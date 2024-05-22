using OnionArch.Application.GlobalResponse;
using OnionArch.Application.View_Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Product.GetProductByCategory
{
    public class GetProductByCategoryResponse:GlobalResponseResult
    {
        public List<VM_Result_ProductLink> CategoryProducts { get; set; }
    }
}
