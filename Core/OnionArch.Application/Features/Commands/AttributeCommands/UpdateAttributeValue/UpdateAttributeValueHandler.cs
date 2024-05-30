using MediatR;
using OnionArch.Application.Abstractions.AttributeServices;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AttributeCommands.UpdateAttributeValue
{
    public class UpdateAttributeValueHandler : IRequestHandler<UpdateAttributeValueRequest, UpdateAttributeValueResponse>
    {
        private readonly IAttributeService _attributeService;

        public UpdateAttributeValueHandler(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        public async Task<UpdateAttributeValueResponse> Handle(UpdateAttributeValueRequest request, CancellationToken cancellationToken)
        {
            

            try
            {
                var result = await _attributeService.UpdateAttributeValueAsync(request.AttributeValueId, request.NewValue);
                if (result == true)
                {
                    return new UpdateAttributeValueResponse()
                    {
                        IsUpdated = true,
                        HassError=false,
                        StatusCode=HttpStatusCode.OK,
                        StatusCodeString= HttpStatusCode.OK.ToString(),
                        Message= "Özelliğin değer güncelleme işlemi başarılı"
                    };
                }
                else
                {
                    return new UpdateAttributeValueResponse()
                    {
                        IsUpdated = false,
                        HassError = false,
                        StatusCode = HttpStatusCode.OK,
                        StatusCodeString = HttpStatusCode.OK.ToString(),
                        Message = "Özelliğin değer güncelleme işlemi başarısız"
                    };
                }
            }
            catch (Exception ex)
            {
                return new UpdateAttributeValueResponse()
                {
                    IsUpdated = false,
                    HassError = true,
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Özelliğin değer güncelleme işlemi başarısız",
                    ErrorMessage=ex.Message.ToString()
                };
            }
        }
    }
}
