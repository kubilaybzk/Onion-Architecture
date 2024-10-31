using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.ProductFilterComands.DeleteCategoryAttributeFilter
{
    public class DeleteCategoryAttributeFilterResponse:GlobalResponseResult
    {
        public Boolean isDeleted { get; set; }
    }
}
