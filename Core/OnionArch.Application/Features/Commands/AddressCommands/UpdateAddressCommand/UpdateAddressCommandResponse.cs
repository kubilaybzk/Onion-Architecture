using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AddressCommands.UpdateAddressCommand
{
    public class UpdateAddressCommandResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
