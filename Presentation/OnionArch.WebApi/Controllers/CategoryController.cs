using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.CategoryCommands.AddCategoryCommands;
using OnionArch.Application.Features.Commands.CategoryCommands.DeleteCategoryComands;
using OnionArch.Application.Features.Commands.CategoryCommands.UpdateCategoryComands;
using OnionArch.Application.Features.Queries.CategoryQueries.GetAllCategory;

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
            return Ok(CategoryResponse);
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCategoryRequest getAllCategoryRequest)
        {
            GetAllCategoryResponse CategoryResponse = await _mediator.Send(getAllCategoryRequest);
            return Ok(CategoryResponse);
        }
        [AllowAnonymous]
        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory([FromQuery] DeleteCategoryRequest deleteCategoryRequest)
        {
            DeleteCategoryResponse CategoryResponse = await _mediator.Send(deleteCategoryRequest);
            return Ok(CategoryResponse);
        }

        [AllowAnonymous]
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryRequest updateCategoryRequest)
        {
            UpdateCategoryResponse CategoryResponse = await _mediator.Send(updateCategoryRequest);
            return Ok(CategoryResponse);
        }
    }
}
