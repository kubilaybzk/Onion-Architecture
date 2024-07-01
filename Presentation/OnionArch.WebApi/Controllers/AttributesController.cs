using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Abstractions.AttributeServices;
using OnionArch.Application.Features.Commands.AttributeCommands.AddAttribute;
using OnionArch.Application.Features.Commands.AttributeCommands.AssignAttributesToProduct;
using OnionArch.Application.Features.Commands.AttributeCommands.DeleteAttribute;
using OnionArch.Application.Features.Commands.AttributeCommands.DeleteAttributeValue;
using OnionArch.Application.Features.Commands.AttributeCommands.RemoveAttributeFromProduct;
using OnionArch.Application.Features.Commands.AttributeCommands.UpdateAttribute;
using OnionArch.Application.Features.Commands.AttributeCommands.UpdateAttributeValue;
using OnionArch.Application.Features.Queries.Attribute.GetAllAttributes;
using OnionArch.Application.Features.Queries.Attribute.GetAllAttributeValues;
using OnionArch.Application.Features.Queries.Attribute.GetProductAttributes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OnionArch.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Admin")]

    public class AttributesController : ControllerBase
    {
        private readonly IAttributeService _attributeService;
        readonly IMediator _mediator;

        public AttributesController(IAttributeService attributeService, IMediator mediator)
        {
            _attributeService = attributeService;
            _mediator = mediator;
        }
        [AllowAnonymous]
        [HttpGet("GetAllAttributes")]
        public async Task<IActionResult> GetAllAttributes([FromQuery] GetAllAttributesRequest request)
        {
            GetAllAttributesResponse attributes = await _mediator.Send(request);
            switch (attributes.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(attributes);
                case HttpStatusCode.BadRequest:
                    return BadRequest(attributes);
                case HttpStatusCode.NotFound:
                    return NotFound(attributes);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)attributes.StatusCode, attributes);
            }
        }
        
        [AllowAnonymous]
        [HttpGet("GetAttributeValues")]
        public async Task<IActionResult> GetAttributeValues([FromQuery]GetAllAttributeValuesRequest request )
        {
            GetAllAttributeValuesResponse attributes = await _mediator.Send(request);
            switch (attributes.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(attributes);
                case HttpStatusCode.BadRequest:
                    return BadRequest(attributes);
                case HttpStatusCode.NotFound:
                    return NotFound(attributes);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)attributes.StatusCode, attributes);
            }
        }
        
        [AllowAnonymous]
        [HttpGet("GetProductAttributes")]
        public async Task<IActionResult> GetProductAttributes([FromQuery] GetProductAttributesRequest request)
        {
            GetProductAttributesResponse attributes = await _mediator.Send(request);
            switch (attributes.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(attributes);
                case HttpStatusCode.BadRequest:
                    return BadRequest(attributes);
                case HttpStatusCode.NotFound:
                    return NotFound(attributes);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)attributes.StatusCode, attributes);
            }
        }
        [AllowAnonymous]
        [HttpPost("AddAttribute")]
        public async Task<IActionResult> AddAttribute([FromBody] AddAttributeRequest request)
        {
            AddAttributeResponse attributes = await _mediator.Send(request);
            switch (attributes.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(attributes);
                case HttpStatusCode.BadRequest:
                    return BadRequest(attributes);
                case HttpStatusCode.NotFound:
                    return NotFound(attributes);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)attributes.StatusCode, attributes);
            }
        }

        [HttpPost("AssignAttributesToProduct")]
        public async Task<IActionResult> AssignAttributesToProduct([FromBody] AssignAttributesToProductRequest request)
        {
            AssignAttributesToProductResponse attributes = await _mediator.Send(request);
            switch (attributes.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(attributes);
                case HttpStatusCode.BadRequest:
                    return BadRequest(attributes);
                case HttpStatusCode.NotFound:
                    return NotFound(attributes);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)attributes.StatusCode, attributes);
            }
        }

        [HttpPut("UpdateAttributeValue")]
        public async Task<IActionResult> UpdateAttributeValue([FromBody] UpdateAttributeValueRequest request)
        {
            UpdateAttributeValueResponse attributes = await _mediator.Send(request);
            switch (attributes.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(attributes);
                case HttpStatusCode.BadRequest:
                    return BadRequest(attributes);
                case HttpStatusCode.NotFound:
                    return NotFound(attributes);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)attributes.StatusCode, attributes);
            }
        }

        [HttpPut("UpdateAttribute")]
        public async Task<IActionResult> UpdateAttribute([FromBody] UpdateAttributeRequest request){
            UpdateAttributeResponse attributes = await _mediator.Send(request);
            switch (attributes.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(attributes);
                case HttpStatusCode.BadRequest:
                    return BadRequest(attributes);
                case HttpStatusCode.NotFound:
                    return NotFound(attributes);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)attributes.StatusCode, attributes);
            }
        }

        [HttpDelete("DeleteAttribute")]
        public async Task<IActionResult> DeleteAttribute ([FromQuery] DeleteAttributeRequest request)
        {
            DeleteAttributeResponse attributes = await _mediator.Send(request);
            switch (attributes.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(attributes);
                case HttpStatusCode.BadRequest:
                    return BadRequest(attributes);
                case HttpStatusCode.NotFound:
                    return NotFound(attributes);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)attributes.StatusCode, attributes);
            }
        }

        [HttpDelete("DeleteAttributeValue")]
        public async Task<IActionResult> DeleteAttributeValue([FromQuery] DeleteAttributeValueRequest request)
        {
            DeleteAttributeValueResponse attributes = await _mediator.Send(request);
            switch (attributes.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(attributes);
                case HttpStatusCode.BadRequest:
                    return BadRequest(attributes);
                case HttpStatusCode.NotFound:
                    return NotFound(attributes);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)attributes.StatusCode, attributes);
            }
        }

        [HttpDelete("RemoveAttributeFromProduct")]
        public async Task<IActionResult> RemoveAttributeFromProduct([FromQuery] RemoveAttributeFromProductRequest request)
        {
            RemoveAttributeFromProductResponse attributes = await _mediator.Send(request);
            switch (attributes.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(attributes);
                case HttpStatusCode.BadRequest:
                    return BadRequest(attributes);
                case HttpStatusCode.NotFound:
                    return NotFound(attributes);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)attributes.StatusCode, attributes);
            }
        }
    }

     

 

}
