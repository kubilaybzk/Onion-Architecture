using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Abstractions.AttributeServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnionArch.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttributesController : ControllerBase
    {
        private readonly IAttributeService _attributeService;

        public AttributesController(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAttributes()
        {
            var attributes = await _attributeService.GetAllAttributesAsync();
            return Ok(attributes);
        }

        [HttpGet("{attributeId}/values")]
        public async Task<IActionResult> GetAttributeValues(Guid attributeId)
        {
            var values = await _attributeService.GetAttributeValuesAsync(attributeId);
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> AddAttribute([FromBody] AddAttributeRequest request)
        {
            var attribute = await _attributeService.AddOrGetAttributeAsync(request.Name);
            var attributeValue = await _attributeService.AddOrGetAttributeValueAsync(attribute.ID, request.DefaultValue);
            return Ok(new { attribute, attributeValue });
        }

        [HttpGet("products/{productId}/attributes")]
        public async Task<IActionResult> GetProductAttributes(Guid productId)
        {
            var attributes = await _attributeService.GetProductAttributesAsync(productId);
            return Ok(attributes);
        }

        [HttpPost("products/{productId}/attributes")]
        public async Task<IActionResult> AssignAttributesToProduct(Guid productId, [FromBody] AssignAttributesRequest request)
        {
            await _attributeService.AssignAttributesToProductAsync(productId, request.AttributeValueIds);
            return Ok();
        }

        [HttpDelete("products/{productId}/attributes/{attributeValueId}")]
        public async Task<IActionResult> RemoveAttributeFromProduct(Guid productId, Guid attributeValueId)
        {
            await _attributeService.RemoveAttributeFromProductAsync(productId, attributeValueId);
            return NoContent();
        }

        [HttpPut("attributes/{attributeValueId}")]
        public async Task<IActionResult> UpdateAttributeValue(Guid attributeValueId, [FromBody] UpdateAttributeValueRequest request)
        {
            await _attributeService.UpdateAttributeValueAsync(attributeValueId, request.NewValue);
            return NoContent();
        }
    }

    public class AddAttributeRequest
    {
        public string Name { get; set; }
        public string DefaultValue { get; set; }
    }

    public class AssignAttributesRequest
    {
        public IEnumerable<Guid> AttributeValueIds { get; set; }
    }

    public class UpdateAttributeValueRequest
    {
        public string NewValue { get; set; }
    }
}
