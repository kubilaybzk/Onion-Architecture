using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.CategoryCommands.AddCategoryCommands;
using OnionArch.Application.Features.Queries.CategoryQueries.DeleteCategory;
using OnionArch.Application.Features.Queries.CategoryQueries.GetAllCategory;
using OnionArch.Application.Features.Queries.Product.GetAllProducts;
using OnionArch.Application.Features.Queries.Product.Product.GetAllProducts;

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
            CreateCategoryResponse productResponse = await _mediator.Send(createCategoryRequest);
            return Ok(productResponse);
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCategoryRequest getAllCategoryRequest)
        {
            GetAllCategoryResponse productResponse = await _mediator.Send(getAllCategoryRequest);
            return Ok(productResponse);
        }
        [AllowAnonymous]
        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory([FromQuery] DeleteCategoryRequest deleteCategoryRequest)
        {
            DeleteCategoryResponse productResponse = await _mediator.Send(deleteCategoryRequest);
            return Ok(productResponse);
        }
    }
}
