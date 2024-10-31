using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.CategoryCommands.DeleteCategoryComands;
using OnionArch.Application.Features.Commands.ProductFilterComands.DeleteCategoryAttributeFilter;
using OnionArch.Application.Features.Commands.ProductFilterComands.SaveCategoryAttributeFilter;
using OnionArch.Application.Features.Queries.ProductAttributesQueries.GetAllProductAttributesWithOutFilter;
using OnionArch.Application.Features.Queries.ProductAttributesQueries.GetCategoriesFilter;
using System.Net;

namespace OnionArch.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAttributeManagerController : ControllerBase
    {
        readonly IMediator _mediator;

        public CategoryAttributeManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("GetAllCategoryProductsAttributes")]
        public async Task<IActionResult> GetAllCategoryAttributes([FromQuery] GetAllCategoryProductsAttributesRequest getAllCategoryProductsAttributesRequest)
        {
            GetAllCategoryProductsAttributesResponse CategoryResponse = await _mediator.Send(getAllCategoryProductsAttributesRequest);

            switch (CategoryResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(CategoryResponse);
                case HttpStatusCode.BadRequest:
                    return BadRequest(CategoryResponse);
                case HttpStatusCode.NotFound:
                    return NotFound(CategoryResponse);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)CategoryResponse.StatusCode, CategoryResponse);
            }
        }

        [HttpPost("SaveCategoryAttributeFilter")]
        public async Task<IActionResult> SaveAllCategoryAttributes([FromBody] CreateCategoryAttributeFilterRequest saveCategoryAttributeFilterRequest)
        {
            CreateCategoryAttributeFilterResponse CategoryResponse = await _mediator.Send(saveCategoryAttributeFilterRequest);

            switch (CategoryResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(CategoryResponse);
                case HttpStatusCode.BadRequest:
                    return BadRequest(CategoryResponse);
                case HttpStatusCode.NotFound:
                    return NotFound(CategoryResponse);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)CategoryResponse.StatusCode, CategoryResponse);
            }
        }

        [HttpGet("GetCategoryFilterBySlug")]
        public async Task<IActionResult> GetCategoryFilterBySlug([FromQuery] GetCategoriesFilterRequest getCategoriesFilterRequest)
        {
            GetCategoriesFilterResponse CategoryResponse = await _mediator.Send(getCategoriesFilterRequest);

            switch (CategoryResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(CategoryResponse);
                case HttpStatusCode.BadRequest:
                    return BadRequest(CategoryResponse);
                case HttpStatusCode.NotFound:
                    return NotFound(CategoryResponse);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)CategoryResponse.StatusCode, CategoryResponse);
            }
        }


        [HttpDelete("DeleteCategoryFilterById")]
        public async Task<IActionResult> DeleteCategoryFilterById([FromQuery] DeleteCategoryAttributeFilterRequest deleteCategoryAttributeFilterRequest)
        {
            DeleteCategoryAttributeFilterResponse CategoryResponse = await _mediator.Send(deleteCategoryAttributeFilterRequest);

            switch (CategoryResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(CategoryResponse);
                case HttpStatusCode.BadRequest:
                    return BadRequest(CategoryResponse);
                case HttpStatusCode.NotFound:
                    return NotFound(CategoryResponse);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)CategoryResponse.StatusCode, CategoryResponse);
            }
        }
    }
}
