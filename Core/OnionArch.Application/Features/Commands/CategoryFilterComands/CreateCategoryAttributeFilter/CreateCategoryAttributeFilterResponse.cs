using OnionArch.Application.Features.Queries.ProductAttributesQueries.GetAllProductAttributesWithOutFilter;
using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.ProductFilterComands.SaveCategoryAttributeFilter
{
    public class CreateCategoryAttributeFilterResponse : GlobalResponseResult
    {
        public Boolean isCreated { get; set; }
    }
 
}
