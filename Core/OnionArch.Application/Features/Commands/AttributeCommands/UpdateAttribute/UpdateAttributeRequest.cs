using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AttributeCommands.UpdateAttribute
{
    public class UpdateAttributeRequest:IRequest<UpdateAttributeResponse>
    {
        public string NewValue { get; set; }
        public Guid AttributeId { get; set; }
    }
}
