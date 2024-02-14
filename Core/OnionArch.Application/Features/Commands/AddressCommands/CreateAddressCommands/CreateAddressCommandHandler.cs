using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.AddressServices;
using OnionArch.Application.View_Models.Addresses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AddressCommands.CreateAddressCommands
{
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommandRequest, CreateAddressCommandResponse>
    {
        public readonly IAddressService _addressService;

        public CreateAddressCommandHandler(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<CreateAddressCommandResponse> Handle(CreateAddressCommandRequest request, CancellationToken cancellationToken)
        {
            VM_Create_Address resquestparams = new VM_Create_Address();
            resquestparams.PhoneNumber = request.PhoneNumber;
            resquestparams.AddressName = request.AddressName;
            resquestparams.Neighbourhood = request.Neighbourhood;
            resquestparams.LongAddress = request.LongAddress;
            resquestparams.City = request.City;
            resquestparams.Country = request.Country;
            resquestparams.District = request.District;

            bool? result = await _addressService.CreateAddressAsync(resquestparams);
            if (result == true)
            {
                return new()
                {
                    Message = "Ekleme işlemi başarılı",
                    StatusCode = StatusCodes.Status201Created
                };
            }
            else
            {
                return new()
                {
                    Message = "Ekleme işlemi başarısız",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
