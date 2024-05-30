using MediatR;
using OnionArch.Application.Abstractions.AttributeServices;
using OnionArch.Application.Features.Commands.AttributeCommands.DeleteAttributeValue;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AttributeCommands.RemoveAttributeFromProduct
{
    public class RemoveAttributeFromProductHandler : IRequestHandler<RemoveAttributeFromProductRequest, RemoveAttributeFromProductResponse>
    {
        private readonly IAttributeService _attributeService;

        public RemoveAttributeFromProductHandler(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        public async Task<RemoveAttributeFromProductResponse> Handle(RemoveAttributeFromProductRequest request, CancellationToken cancellationToken)
        {
            

            try
            {
                var result = await _attributeService.RemoveAttributeFromProductAsync(request.ProductId, request.AttributeValueId);
                if (result == true)
                {
                    return new RemoveAttributeFromProductResponse()
                    {
                        IsDeleted = true,
                        HassError = false,
                        StatusCode = HttpStatusCode.OK,
                        StatusCodeString = HttpStatusCode.OK.ToString(),
                        Message = "Özellik üründen başarıyla kaldırıldı."
                    };
                }
                else
                {
                    return new RemoveAttributeFromProductResponse()
                    {
                        IsDeleted = false,
                        HassError = false,
                        StatusCode = HttpStatusCode.OK,
                        StatusCodeString = HttpStatusCode.OK.ToString(),
                        Message = "Özellik üründen  kaldırılırken hata alındı."
                    };
                }
            }
            catch (Exception ex)
            {
                return new RemoveAttributeFromProductResponse()
                {
                    IsDeleted = false,
                    HassError = true,
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Özellik üründen  kaldırılırken hata alındı."
                };
            }
        }
    }
}
