using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.AddressServices;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            try
            {
                var userAddresses = await _addressService.GetUserAddressesAsync();
                if (userAddresses.Count > 0)
                {
                    return new GetAddressResonse()
                    {
                        Addresses = userAddresses,
                        HassError = false,
                        Message = "Addresler başarıyla listelendi.",
                        StatusCode = HttpStatusCode.OK,
                        StatusCodeString = HttpStatusCode.OK.ToString()

                    };
                }
                else
                {
                    return new GetAddressResonse()
                    {
                        Addresses = null,
                        HassError = false,
                        Message ="Addres listesinde bir address bulunamadı",
                        StatusCode = HttpStatusCode.OK,
                        StatusCodeString = HttpStatusCode.OK.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                return new GetAddressResonse()
                {
                    Addresses = null,
                    HassError=true,
                    Message = "Addres listelenirken bir hata ile karşılaşıldı.",
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
