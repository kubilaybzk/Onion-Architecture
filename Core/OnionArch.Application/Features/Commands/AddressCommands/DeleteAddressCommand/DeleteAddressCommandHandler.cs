using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.AddressServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            try
            {
                bool? result = await _addressService.DeleteAddressAsync(request.DeletedAddressId);
                if (result == true)
                {
                    return new()
                    {
                        Message = "Addres Silme işlemi başarılı",
                        StatusCode = HttpStatusCode.OK,
                        StatusCodeString=HttpStatusCode.OK.ToString(),
                        HassError=false
                    };
                }
                else
                {
                    return new()
                    {
                        Message = "Addres Silme işlemi başarısız",
                        StatusCode = HttpStatusCode.BadRequest,
                        StatusCodeString = HttpStatusCode.BadRequest.ToString(),
                        HassError=true
                    };
                }
            }
            catch (Exception ex)
            {
                return new()
                {
                    Message = "Addres Silme işlemi başarısız sunucu taraflı bir hata",
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                    HassError = true,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
