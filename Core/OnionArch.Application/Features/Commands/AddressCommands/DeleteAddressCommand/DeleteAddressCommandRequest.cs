using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AddressCommands.DeleteAddressCommand
{
    public class DeleteAddressCommandRequest:IRequest<DeleteAddressCommandResponse>
    {
        public string DeletedAddressId { get; set; }
    }
}
