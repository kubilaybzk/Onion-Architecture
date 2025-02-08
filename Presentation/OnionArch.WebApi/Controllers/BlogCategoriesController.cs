using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.BlogCategoryCommands.CreateBlogCategory;
using OnionArch.Application.Features.Commands.BlogCategoryCommands.DeleteBlogCategory;
using OnionArch.Application.Features.Commands.BlogCategoryCommands.UpdateBlogCategory;
using OnionArch.Application.Features.Queries.BlogCategoryQueries.GetAllBlogCategories;

namespace OnionArch.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogCategoriesController : ControllerBase
    {
        readonly IMediator _mediator;

        public BlogCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllBlogCategories")]
        public async Task<IActionResult> GetAllBlogCategories([FromQuery] GetAllBlogCategoriesQueryRequest request)
        {
            GetAllBlogCategoriesQueryResponse response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("CreateBlogCategory")]
       
        public async Task<IActionResult> CreateBlogCategory([FromBody] CreateBlogCategoryCommandRequest request)
        {
            CreateBlogCategoryCommandResponse response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("UpdateBlogCategory")]
       
        public async Task<IActionResult> UpdateBlogCategory([FromBody] UpdateBlogCategoryCommandRequest request)
        {
            
            UpdateBlogCategoryCommandResponse response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("DeleteBlogCategory")]
        
        public async Task<IActionResult> DeleteBlogCategory([FromQuery] DeleteBlogCategoryCommandRequest request)
        {
            DeleteBlogCategoryCommandResponse response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
