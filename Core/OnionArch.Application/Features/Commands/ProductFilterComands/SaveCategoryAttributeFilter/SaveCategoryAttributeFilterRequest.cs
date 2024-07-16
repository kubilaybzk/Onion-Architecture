using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.ProductFilterComands.SaveCategoryAttributeFilter
{
    public class SaveCategoryAttributeFilterRequest : IRequest<SaveCategoryAttributeFilterResponse>
    {
        public string JsonResult { get; set; }
        public string CategorySlug { get; set; }

        public int Order { get; set; } = 0;
    }

}
