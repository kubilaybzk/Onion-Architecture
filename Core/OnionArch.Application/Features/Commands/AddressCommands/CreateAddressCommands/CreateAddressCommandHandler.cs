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

            try
            {
                bool? result = await _addressService.CreateAddressAsync(resquestparams);
                if (result == true)
                {
                    return new()
                    {
                        Message = "Adress Ekleme işlemi başarılı",
                        StatusCode = System.Net.HttpStatusCode.OK,
                        ErrorMessage = "",
                        HassError = false,
                        StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                    };
                }
                else
                {
                    return new()
                    {
                        Message = "Adress Ekleme işlemi başarısız",
                        StatusCode = System.Net.HttpStatusCode.NotAcceptable,
                        HassError=true,
                        ErrorMessage="Ekleme yapılamadı",
                        StatusCodeString = System.Net.HttpStatusCode.NotAcceptable.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                return new()
                {
                    Message = "Adress Ekleme işlemi başarısız",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    HassError = true,
                    ErrorMessage = ex.Message.ToString(),
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString()
                };

            }
        }
    }
}
