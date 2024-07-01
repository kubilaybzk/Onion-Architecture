using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AttributeCommands.AssignAttributesToProduct
{
    public class AssignAttributesToProductRequest:IRequest<AssignAttributesToProductResponse>
    {
        public Guid AttributeValueIds { get; set; }
        public Guid ProductId { get; set; }
    }
}
