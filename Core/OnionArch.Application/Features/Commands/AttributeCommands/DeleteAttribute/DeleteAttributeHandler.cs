using MediatR;
using OnionArch.Application.Abstractions.AttributeServices;
using OnionArch.Application.Features.Commands.AttributeCommands.UpdateAttribute;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AttributeCommands.DeleteAttribute
{
    public class DeleteAttributeHandler : IRequestHandler<DeleteAttributeRequest, DeleteAttributeResponse>
    {
        private readonly IAttributeService _attributeService;

        public DeleteAttributeHandler(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        public async Task<DeleteAttributeResponse> Handle(DeleteAttributeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _attributeService.DeleteAttributeAsync(request.attributeId);
                if (result == true)
                {
                    return new DeleteAttributeResponse()
                    {
                        IsDeleted = true,
                        HassError = false,
                        StatusCode = HttpStatusCode.OK,
                        StatusCodeString = HttpStatusCode.OK.ToString(),
                        Message = "Özelliğin  silme işlemi başarılı"
                    };
                }
                else
                {
                    return new DeleteAttributeResponse()
                    {
                        IsDeleted = false,
                        HassError = false,
                        StatusCode = HttpStatusCode.OK,
                        StatusCodeString = HttpStatusCode.OK.ToString(),
                        Message = "Özelliğin  silme işlemi başarısız"
                    };
                }
            }
            catch (Exception ex)
            {
                return new DeleteAttributeResponse()
                {
                    IsDeleted = false,
                    HassError = true,
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Özelliğin  silme işlemi başarısız",
                    ErrorMessage=ex.Message
                };
            }
        }
    }
}
