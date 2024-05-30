using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AttributeCommands.AddAttribute
{
    public class AddAttributeRequest:IRequest<AddAttributeResponse>
    {
        public string Name { get; set; }
        public string DefaultValue { get; set; }
    }
}
