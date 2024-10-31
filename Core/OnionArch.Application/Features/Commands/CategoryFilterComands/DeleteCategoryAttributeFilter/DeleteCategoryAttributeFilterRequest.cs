using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.ProductFilterComands.DeleteCategoryAttributeFilter
{
    public  class DeleteCategoryAttributeFilterRequest:IRequest<DeleteCategoryAttributeFilterResponse>
    {
        public string CategoryFilterId { get; set; }
    }
}
