using OnionArch.Application.GlobalResponse;
using OnionArch.Application.View_Models.CategoryFilter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.ProductAttributesQueries.GetCategoriesFilter
{
    public class GetCategoriesFilterResponse:GlobalResponseResult
    {
        public List<VM_Category_Filters_Result> CategoryFilters { get; set; }
    }
}
