using MediatR;
using OnionArch.Application.Features.Commands.AttributeCommands.DeleteAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AttributeCommands.DeleteAttributeValue
{
    public class DeleteAttributeValueRequest: IRequest<DeleteAttributeValueResponse>
    {
        public Guid AttributeValueId { get; set; }
    }
}
