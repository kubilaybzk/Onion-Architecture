using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AttributeCommands.UpdateAttributeValue
{
    public class UpdateAttributeValueResponse:GlobalResponseResult
    {
        public bool IsUpdated {  get; set; } 
    }
}
