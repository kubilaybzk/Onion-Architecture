using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AttributeCommands.DeleteAttribute
{
    public class DeleteAttributeRequest:IRequest<DeleteAttributeResponse>
    {
        public Guid attributeId { get; set; }
    }
}
