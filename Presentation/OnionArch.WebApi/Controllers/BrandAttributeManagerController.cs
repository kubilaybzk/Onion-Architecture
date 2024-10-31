using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.BrandFilterComands.CreateBrandAttributeFilter;
using OnionArch.Application.Features.Queries.BrandFilterQueries.GetAllBrandProductsAttributes;
using OnionArch.Application.Features.Queries.BrandFilterQueries.GetBrandFilters;
using System.Net;

namespace OnionArch.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandAttributeManagerController : ControllerBase
    {
        readonly IMediator _mediator;

        public BrandAttributeManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("CreateBrandAttributeFilter")]
        public async Task<IActionResult> CreateBrandAttributeFilter([FromBody] CreateBrandAttributeFilterRequest createBrandAttributeFilterRequest)
        {
            CreateBrandAttributeFilterResponse result = await _mediator.Send(createBrandAttributeFilterRequest);
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

        //Marka bilgisine göre ürün listelerken filtreleri getiren alan.
        [AllowAnonymous]
        [HttpGet("GetBrandFilters")]
        public async Task<IActionResult> GetBrandFilters([FromQuery] GetBrandFiltersRequest getBrandFiltersRequest)
        {
            GetBrandFiltersResponse result = await _mediator.Send(getBrandFiltersRequest);
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

        //Marka bilgisine göre ürün listelerken kullanılabilecek tüm ürünleri listeleyen alan.
        [AllowAnonymous]
        [HttpGet("GetAllBrandProductsAttributes")]
        public async Task<IActionResult> GetAllBrandProductsAttributes([FromQuery] GetAllBrandProductsAttributesRequest getAllBrandProductsAttributesRequest)
        {
            GetAllBrandProductsAttributesResponse result = await _mediator.Send(getAllBrandProductsAttributesRequest);
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
