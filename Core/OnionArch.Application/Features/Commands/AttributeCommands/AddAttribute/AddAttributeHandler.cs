using MediatR;
using OnionArch.Application.Abstractions.AttributeServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AttributeCommands.AddAttribute
{
    public class AddAttributeHandler : IRequestHandler<AddAttributeRequest, AddAttributeResponse>
    {
        private readonly IAttributeService _attributeService;

        public AddAttributeHandler(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        public async Task<AddAttributeResponse> Handle(AddAttributeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var attribute = await _attributeService.AddOrGetAttributeAsync(request.Name);
                var attributeValue = await _attributeService.AddOrGetAttributeValueAsync(attribute.ID, request.DefaultValue);

                return new  AddAttributeResponse(){
                    IsAdded=true,
                    Message="Atribute başarıyla eklendi",
                    HassError=false,
                    StatusCode=HttpStatusCode.OK,
                    StatusCodeString=HttpStatusCode.OK.ToString(),
                };
            }
            catch (Exception ex)
            {
                return new AddAttributeResponse()
                {
                    IsAdded=false,
                    ErrorMessage=ex.Message.ToString(),
                    Message = "Atribute başarıyla eklendi",
                    HassError = false,
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString(),
                };
            }
        }
    }
}
