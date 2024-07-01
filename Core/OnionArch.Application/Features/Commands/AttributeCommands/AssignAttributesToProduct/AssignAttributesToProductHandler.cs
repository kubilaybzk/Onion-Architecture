using MediatR;
using OnionArch.Application.Abstractions.AttributeServices;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AttributeCommands.AssignAttributesToProduct
{
    public class AssignAttributesToProductHandler : IRequestHandler<AssignAttributesToProductRequest, AssignAttributesToProductResponse>
    {
        private readonly IAttributeService _attributeService;

        public AssignAttributesToProductHandler(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        public async Task<AssignAttributesToProductResponse> Handle(AssignAttributesToProductRequest request, CancellationToken cancellationToken)
        {
            try {
                var result = await _attributeService.AssignAttributesToProductAsync(request.ProductId, request.AttributeValueIds);
                if (result == true)
                {
                    return new AssignAttributesToProductResponse()
                    {
                        IsAssign = true,
                        HassError = false,
                        Message = "Özellik Başarılı bir şekilde ürün atandı veya güncellendi.",
                        StatusCode = System.Net.HttpStatusCode.OK,
                        StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                    };
                }
                else
                {
                    return new AssignAttributesToProductResponse()
                    {
                        HassError = true,
                        IsAssign = false,
                        Message = "Özellik ürüne atanamadı.",
                        StatusCode = System.Net.HttpStatusCode.OK,
                        StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                    };
                }
            }
            catch (Exception ex)
            {
                return new AssignAttributesToProductResponse()
                {
                    HassError = true,
                    IsAssign = false,
                    Message = "Özellik ürüne atanamadı sunucu taraflı bir hata",
                    ErrorMessage = ex.Message.ToString(),
                    StatusCode=System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString(),
                };
            }
            
        }
    }
}
