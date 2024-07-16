using OnionArch.Application.Features.Queries.ProductAttributesQueries.GetAllProductAttributesWithOutFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.View_Models.CategoryFilter
{
    public class VM_Category_Filters_Result
    {
        public string? CategoryFilterId { get; set; }
        public string  AttributeName { get; set; }
        public string AttributeNameSlug { get; set; }
        public string AttributeId { get; set; }
        public List<VM_Category_FilterValue_Result>? AttributeValues { get; set; }
        public string FilterType { get; set; }
        public int Order {  get; set; }
    }

    public class VM_Category_FilterValue_Result
    {
        public string AttributeValue { get; set; }
        public string AttributeValueSlug { get; set; }
        public string AttributeValueId { get; set; }
    }
}
