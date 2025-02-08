using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.BlogCommands.CreateBlogCommands;
using OnionArch.Application.Features.Commands.BlogCommands.DeleteBlogCommands;
using OnionArch.Application.Features.Commands.BlogCommands.UpdateBlogCommands;
using OnionArch.Application.Features.Queries.BlogQueries.GetBlogById;
using OnionArch.Application.Features.Queries.BlogQueries.GetAllBlogs;

namespace OnionArch.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateBlog")]
        public async Task<IActionResult> CreateBlog([FromForm] CreateBlogCommandRequest request)
        {
            CreateBlogCommandResponse response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] GetBlogByIdQueryRequest request)
        {
            GetBlogByIdQueryResponse response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        
        [HttpPut("UpdateBlog")]
        public async Task<IActionResult> Update([FromForm] UpdateBlogCommandRequest request)
        {

            UpdateBlogCommandResponse response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

       
        [HttpDelete("DeleteBlog")]
        public async Task<IActionResult> Delete([FromQuery] DeleteBlogCommandRequest request)
        {
            DeleteBlogCommandResponse response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("GetAllBlogList")]
        public async Task<IActionResult> GetAllBlogList([FromQuery] GetAllBlogsQueryRequest request)
        {
            GetAllBlogsQueryResponse response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}