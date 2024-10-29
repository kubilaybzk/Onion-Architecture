using OnionArch.Application.GlobalResponse;
using OnionArch.Application.View_Models.Product;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.BrandQueries.GetAllProductByBrandNameQueries
{
    public class GetAllProductByBrandNameResponse: GlobalResponseResult
    {
        public List<VM_Result_ProductLink>? Products { get; set; }
        public int TotalCount { get; set; }
        public int TotalPageSize { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrev { get; set; }
        public int PageSize { get; set; }
        public Brand? BrandInfo { get; set; }
    }
}
