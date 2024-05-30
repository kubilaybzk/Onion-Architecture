using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AttributeCommands.UpdateAttribute
{
    public class UpdateAttributeResponse:GlobalResponseResult
    {
        public bool IsUpdated { get; set; }
    }
}
