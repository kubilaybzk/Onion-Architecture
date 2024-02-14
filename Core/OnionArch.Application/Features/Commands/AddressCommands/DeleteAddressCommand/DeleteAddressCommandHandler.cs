using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.AddressServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AddressCommands.DeleteAddressCommand
{
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommandRequest, DeleteAddressCommandResponse>
    {
        public readonly IAddressService _addressService;

        public DeleteAddressCommandHandler(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<DeleteAddressCommandResponse> Handle(DeleteAddressCommandRequest request, CancellationToken cancellationToken)
        {
            bool? result = await _addressService.DeleteAddressAsync(request.DeletedAddressId);
            if (result == true)
            {
                return new()
                {
                    Message = "Silme işlemi başarılı",
                    StatusCode = StatusCodes.Status200OK
                };
            }
            else
            {
                return new()
                {
                    Message = "Silme işlemi başarısız",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
