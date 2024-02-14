using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.AddressServices;
using OnionArch.Application.Repositories.AddressCrud;
using OnionArch.Application.View_Models.Addresses;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AddressCommands.UpdateAddressCommand
{
    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommandRequest, UpdateAddressCommandResponse>
    {
        public readonly IAddressService _addressService;
        public readonly IAddressWriteRepository _addressWriteRepository;
        public readonly IAddressReadRepository _addressReadRepository;

        public UpdateAddressCommandHandler(IAddressService addressService, IAddressWriteRepository addressWriteRepository, IAddressReadRepository addressReadRepository)
        {
            _addressService = addressService;
            _addressWriteRepository = addressWriteRepository;
            _addressReadRepository = addressReadRepository;
        }

        public async Task<UpdateAddressCommandResponse> Handle(UpdateAddressCommandRequest request, CancellationToken cancellationToken)
        {
            Address currentAddres= await _addressReadRepository.GetByIdAsync(request.Id);

            currentAddres.PhoneNumber = request.PhoneNumber;
            currentAddres.AddressName = request.AddressName;
            currentAddres.Neighbourhood = request.Neighbourhood;
            currentAddres.LongAddress = request.LongAddress;
            currentAddres.City = request.City;
            currentAddres.Country = request.Country;
            currentAddres.District = request.District;
            int result = await _addressWriteRepository.SaveAsync();

            if (result > 0)
            {
                return new()
                {
                    Message = "Düzenleme başarılı",
                    StatusCode = StatusCodes.Status200OK
                };
            }
            else
            {
                return new()
                {
                    Message = "Düzenleme başarısız",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

        }
    }
}
