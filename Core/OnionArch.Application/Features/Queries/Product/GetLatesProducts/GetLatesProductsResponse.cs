using OnionArch.Application.CQRS_Globals.Pagination;
using OnionArch.Application.GlobalResponse;
using OnionArch.Application.View_Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Product.GetLatesProducts
{
    public class GetLatesProductsResponse :GlobalPaginationResponse
    {
        public List<VM_Result_ProductLink> Products { get; set; }
    }
}
