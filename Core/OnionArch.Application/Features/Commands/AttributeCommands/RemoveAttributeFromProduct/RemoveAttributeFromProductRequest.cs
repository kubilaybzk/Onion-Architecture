using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AttributeCommands.RemoveAttributeFromProduct
{
    public class RemoveAttributeFromProductRequest:IRequest<RemoveAttributeFromProductResponse>
    {
        public Guid ProductId { get; set; }
        public Guid AttributeValueId { get; set; }
    }
}
