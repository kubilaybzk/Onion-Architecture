using MediatR;
using OnionArch.Application.Abstractions.AttributeServices;
using OnionArch.Application.Repositories.AttributeCrud.AttributeCrud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Attribute.GetAllAttributes
{
    public class GetAllAttributesHandler : IRequestHandler<GetAllAttributesRequest, GetAllAttributesResponse>
    {
        private readonly IAttributeService _attributeService;

        public GetAllAttributesHandler(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        public async Task<GetAllAttributesResponse> Handle(GetAllAttributesRequest request, CancellationToken cancellationToken)
        {


            try
            {
                var attributes = await _attributeService.GetAllAttributesAsync();


                return new GetAllAttributesResponse()
                {
                    Attributes = attributes.ToList(),
                    HassError = false,
                    Message = "Attributelar başarıyla listelendi",
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString(),
                };


            }
            catch (Exception ex)
            {
                return new GetAllAttributesResponse()
                {
                    Attributes = null,
                    HassError = true,
                    Message = "Attributelar listelenirken bir hata alındı",
                    ErrorMessage = ex.Message.ToString(),
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                };

            }


        }
    }
}
