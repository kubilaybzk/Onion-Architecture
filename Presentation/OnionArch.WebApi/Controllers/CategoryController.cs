using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.CategoryCommands.AddCategoryCommands;
using OnionArch.Application.Features.Commands.CategoryCommands.DeleteCategoryComands;
using OnionArch.Application.Features.Commands.CategoryCommands.UpdateCategoryComands;
using OnionArch.Application.Features.Queries.CategoryQueries.GetAllCategory;
using OnionArch.Application.Features.Queries.CategoryQueries.GetOnlyCategoryName;
using System.Net;

namespace OnionArch.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class CategoryController : ControllerBase
    {
        readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [AllowAnonymous]
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryRequest createCategoryRequest)
        {
            CreateCategoryResponse CategoryResponse = await _mediator.Send(createCategoryRequest);
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

        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCategoryRequest getAllCategoryRequest)
        {
            GetAllCategoryResponse CategoryResponse = await _mediator.Send(getAllCategoryRequest);



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
        [AllowAnonymous]
        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory([FromQuery] DeleteCategoryRequest deleteCategoryRequest)
        {
            DeleteCategoryResponse CategoryResponse = await _mediator.Send(deleteCategoryRequest);

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

        [AllowAnonymous]
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryRequest updateCategoryRequest)
        {
            UpdateCategoryResponse CategoryResponse = await _mediator.Send(updateCategoryRequest);

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


        [AllowAnonymous]
        [HttpGet("GetOnlyCategoryName")]
        public async Task<IActionResult> GetOnlyCategoryName([FromQuery] GetOnlyCategoryNameRequest getOnlyCategoryNameRequest)
        {
            GetOnlyCategoryNameResponse CategoryResponse = await _mediator.Send(getOnlyCategoryNameRequest);



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
