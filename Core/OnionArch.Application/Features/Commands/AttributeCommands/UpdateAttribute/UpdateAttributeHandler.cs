using MediatR;
using OnionArch.Application.Abstractions.AttributeServices;
using OnionArch.Application.Features.Commands.AttributeCommands.UpdateAttributeValue;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AttributeCommands.UpdateAttribute
{
    public class UpdateAttributeHandler : IRequestHandler<UpdateAttributeRequest, UpdateAttributeResponse>
    {
        private readonly IAttributeService _attributeService;

        public UpdateAttributeHandler(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        public async Task<UpdateAttributeResponse> Handle(UpdateAttributeRequest request, CancellationToken cancellationToken)
        {
            try {
                var result = await _attributeService.UpdateAttributeAsync(request.AttributeId, request.NewValue);
                if (result == true)
                {
                    return new UpdateAttributeResponse()
                    {
                        IsUpdated = true,
                        HassError = false,
                        StatusCode = HttpStatusCode.OK,
                        StatusCodeString = HttpStatusCode.OK.ToString(),
                        Message = "Özelliğin  güncelleme işlemi başarılı"
                    };
                }
                else
                {
                    return new UpdateAttributeResponse()
                    {
                        IsUpdated = false,
                        HassError = false,
                        StatusCode = HttpStatusCode.OK,
                        StatusCodeString = HttpStatusCode.OK.ToString(),
                        Message = "Özelliğin  güncelleme işlemi başarısız"
                    };
                }
            }
            catch(Exception ex)
            {
                return new UpdateAttributeResponse()
                {
                    IsUpdated = false,
                    HassError = true,
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Özelliğin  güncelleme işlemi başarısız",
                    ErrorMessage = ex.Message.ToString()
                };
            }

            
            
        }
    }
}
