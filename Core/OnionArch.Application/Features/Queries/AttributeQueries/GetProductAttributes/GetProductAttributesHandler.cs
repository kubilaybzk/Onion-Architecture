using MediatR;
using OnionArch.Application.Abstractions.AttributeServices;
using OnionArch.Application.Features.Queries.Attribute.GetAllAttributeValues;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OnionArch.Application.Features.Queries.Attribute.GetProductAttributes
{
    public class GetProductAttributesHandler : IRequestHandler<GetProductAttributesRequest, GetProductAttributesResponse>
    {
        private readonly IAttributeService _attributeService;

        public GetProductAttributesHandler(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        public async Task<GetProductAttributesResponse> Handle(GetProductAttributesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var attributes = await _attributeService.GetProductAttributesAsync(request.productId);
                return new GetProductAttributesResponse()
                {
                    ProductAttributes = attributes.ToList(),
                    HassError = false,
                    Message = "Ürüne özel özellikler başarıyla listelendi",
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString(),
                };
            }
            catch (Exception ex)
            {
                return new GetProductAttributesResponse()
                {
                    ProductAttributes = null,
                    HassError = true,
                    Message = "Ürüne özel özellikler başarıyla listelenirken hata",
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                };
            }
        }
    }
}
