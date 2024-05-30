using MediatR;
using OnionArch.Application.Abstractions.AttributeServices;
using OnionArch.Application.Features.Queries.Attribute.GetAllAttributes;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OnionArch.Application.Features.Queries.Attribute.GetAllAttributeValues
{
    public class GetAllAttributeValuesHandler : IRequestHandler<GetAllAttributeValuesRequest, GetAllAttributeValuesResponse>
    {
        private readonly IAttributeService _attributeService;

        public GetAllAttributeValuesHandler(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        public async Task<GetAllAttributeValuesResponse> Handle(GetAllAttributeValuesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var attributesValues = await _attributeService.GetAttributeValuesAsync(request.AttributeId);
                return new GetAllAttributeValuesResponse()
                {
                    AttributeValues = attributesValues.ToList(),
                    HassError = false,
                    Message = "AttributeValue lar başarıyla listelendi",
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString(),
                };
            }
            catch (Exception ex)
            {
                return new GetAllAttributeValuesResponse()
                {
                    AttributeValues = null,
                    HassError = true,
                    Message = "AttributeValue listelenirken hata",
                    ErrorMessage = ex.Message.ToString(),
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString(),
                };
            }
        }
    }
}
