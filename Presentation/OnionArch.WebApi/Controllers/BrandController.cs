using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.BrandCommands.CreateBrandCommands;
using OnionArch.Application.Features.Commands.BrandCommands.DeleteBrandCommands;
using OnionArch.Application.Features.Commands.BrandCommands.UpdateBrandCommands;
using OnionArch.Application.Features.Commands.CategoryCommands.AddCategoryCommands;
using OnionArch.Application.Features.Commands.CategoryCommands.DeleteCategoryComands;
using OnionArch.Application.Features.Commands.CategoryCommands.UpdateCategoryComands;
using OnionArch.Application.Features.Commands.Product.CreateOneProductWithImage;
using OnionArch.Application.Features.Queries.BrandQueries.GetAllBrandNameQueries;
using OnionArch.Application.Features.Queries.BrandQueries.GetAllBrands;
using OnionArch.Application.Features.Queries.BrandQueries.GetBrandNameWithIdQueries;
using OnionArch.Application.Features.Queries.CategoryQueries.GetAllCategory;
using OnionArch.Application.Features.Queries.CategoryQueries.GetOnlyCategoryName;
using System.Net;

namespace OnionArch.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class BrandManager : ControllerBase
    {
        readonly IMediator _mediator;

        public BrandManager(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("CreateBrand")]
        public async Task<IActionResult> CreateBrand([FromForm] CreateBrandCommandsRequest createBrandCommandsRequest)
        {
            createBrandCommandsRequest.BrandLogo = Request.Form.Files;
            CreateBrandCommandsResponse result = await _mediator.Send(createBrandCommandsRequest);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                case HttpStatusCode.BadRequest:
                    return BadRequest(result);
                case HttpStatusCode.NotFound:
                    return NotFound(result);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)result.StatusCode, result);
            }

        }
        
        [AllowAnonymous]
        [HttpGet("GetAllBrands")]
        public async Task<IActionResult> GetAllBrands([FromQuery]GetAllBrandsRequest getAllBrandsRequest)
        {
            GetAllBrandsResponse result = await _mediator.Send(getAllBrandsRequest);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                case HttpStatusCode.BadRequest:
                    return BadRequest(result);
                case HttpStatusCode.NotFound:
                    return NotFound(result);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)result.StatusCode, result);
            }

        }
        
        [AllowAnonymous]
        [HttpPut("UpdateBrand")]
        public async Task<IActionResult> UpdateBrand([FromForm] UpdateBrandCommandsRequest updateBrandCommandsRequest)
        {
            UpdateBrandCommandsResponse result = await _mediator.Send(updateBrandCommandsRequest);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                case HttpStatusCode.BadRequest:
                    return BadRequest(result);
                case HttpStatusCode.NotFound:
                    return NotFound(result);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)result.StatusCode, result);
            }

        }
       
        [AllowAnonymous]
        [HttpDelete("DeleteBrand")]
        public async Task<IActionResult> DeleteBrand([FromQuery] DeleteBrandCommandsRequest deleteBrandCommandsRequest)
        {
            DeleteBrandCommandsResponse result = await _mediator.Send(deleteBrandCommandsRequest);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                case HttpStatusCode.BadRequest:
                    return BadRequest(result);
                case HttpStatusCode.NotFound:
                    return NotFound(result);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)result.StatusCode, result);
            }

        }


        [AllowAnonymous]
        [HttpGet("GetAllBrandNameAndId")]
        public async Task<IActionResult> GetAllBrandNameAndId([FromQuery] GetAllBrandNameRequest getAllBrandNameRequest)
        {
            GetAllBrandNameResponse result = await _mediator.Send(getAllBrandNameRequest);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                case HttpStatusCode.BadRequest:
                    return BadRequest(result);
                case HttpStatusCode.NotFound:
                    return NotFound(result);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)result.StatusCode, result);
            }

        }
    }
}
