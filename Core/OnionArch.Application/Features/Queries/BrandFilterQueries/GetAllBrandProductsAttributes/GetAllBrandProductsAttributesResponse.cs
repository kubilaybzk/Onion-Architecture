using OnionArch.Application.GlobalResponse;
using OnionArch.Application.View_Models.CategoryFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.BrandFilterQueries.GetAllBrandProductsAttributes
{
    public  class GetAllBrandProductsAttributesResponse: GlobalResponseResult
    {
        public List<VM_Category_Filters_Result> BrandFilters { get; set; }
    }
}
