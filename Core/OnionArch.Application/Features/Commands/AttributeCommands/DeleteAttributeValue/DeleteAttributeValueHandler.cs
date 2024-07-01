using MediatR;
using OnionArch.Application.Abstractions.AttributeServices;
using OnionArch.Application.Features.Commands.AttributeCommands.DeleteAttribute;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AttributeCommands.DeleteAttributeValue
{
    public class DeleteAttributeValueHandler : IRequestHandler<DeleteAttributeValueRequest, DeleteAttributeValueResponse>
    {
        private readonly IAttributeService _attributeService;

        public DeleteAttributeValueHandler(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        public async Task<DeleteAttributeValueResponse> Handle(DeleteAttributeValueRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _attributeService.DeleteAttributeValueAsync(request.AttributeValueId);
                if (result == true)
                {
                    return new DeleteAttributeValueResponse()
                    {
                        IsDeleted = true,
                        HassError = false,
                        StatusCode = HttpStatusCode.OK,
                        StatusCodeString = HttpStatusCode.OK.ToString(),
                        Message = "Özelliğin değerini  silme işlemi başarılı"
                    };
                }
                else
                {
                    return new DeleteAttributeValueResponse()
                    {
                        IsDeleted = false,
                        HassError = false,
                        StatusCode = HttpStatusCode.OK,
                        StatusCodeString = HttpStatusCode.OK.ToString(),
                        Message = "Özelliğin değerini  silme işlemi başarısız"
                    };
                }
            }
            catch (Exception ex)
            {
                return new DeleteAttributeValueResponse()
                {
                    IsDeleted = false,
                    HassError = true,
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Özelliğin değerini  silme işlemi başarısız",
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
