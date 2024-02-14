using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.AddressServices;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Address.GetAddress
{
    public class GetAddressHandler : IRequestHandler<GetAddressRequest, GetAddressResonse>
    {
        public readonly IAddressService _addressService;

        public GetAddressHandler(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<GetAddressResonse> Handle(GetAddressRequest request, CancellationToken cancellationToken)
        {
            var userAddresses= await _addressService.GetUserAddressesAsync();
           if(userAddresses.Count > 0)
            {
                return new GetAddressResonse()
                {
                    Addresses = userAddresses,
                    Message = "Başarılı",
                    StatusCode = StatusCodes.Status200OK
                };
            }
            else
            {
                return new GetAddressResonse()
                {
                    Addresses = null,
                    Message = "Başarısız",
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
        }
    }
}
