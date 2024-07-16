using OnionArch.Application.GlobalResponse;
using OnionArch.Application.View_Models.CategoryFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.ProductAttributesQueries.GetAllProductAttributesWithOutFilter
{
    public class GetAllProductAttributesWithOutFilterResponse : GlobalResponseResult
    {
        public List<VM_Category_Filters_Result> CategoryFilters { get; set; }
        public string CategoryName { get; set; }
        public string MaterializedPath { get; set; }
        //public List<AttributeWithValues> Attributes { get; set; }
        public int TotalAttributeCount { get; set; }
    }

    //public class AttributeWithValues
    //{
    //    public Guid AttributeId { get; set; }
    //    public string AttributeName { get; set; }

    //    public string AttributeNameSlug { get; set; }
    //    public List<AttributeValue> AttributeValues { get; set; }
    //}

    //public class AttributeValue
    //{
    //    public Guid AttributeValueId { get; set; }
    //    public string Value { get; set; }

    //    public string AttributeValueSlug { get; set; }
    //}
}
