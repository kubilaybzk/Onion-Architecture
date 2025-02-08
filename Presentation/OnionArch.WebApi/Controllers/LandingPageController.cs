using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.AppUser.CreateUser;
using OnionArch.Application.Features.Commands.AppUser.LoginUser;
using OnionArch.Application.Features.Commands.AppUser.LoginUser.FacebookLogin;
using OnionArch.Application.Features.Commands.AppUser.LoginUser.GoogleLogin;
using OnionArch.Application.Features.Commands.AppUser.LoginUser.RefreshTokenLogin;
using OnionArch.Application.Features.Commands.HeroSectionComands.CreateHeroSectionComands;
using OnionArch.Application.Features.Commands.HeroSectionComands.DeleteHeroSectionQueries;
using OnionArch.Application.Features.Commands.HeroSectionComands.UpdateHeroSectionQueries;
using OnionArch.Application.Features.Queries.HeroSectionQueries.GetHeroSectionQueries;
using System.Net;

namespace OnionArch.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandingPageController : ControllerBase
    {
        //Her şeyden önce ilk olarak mediator nesnemizi burada oluşturalım ve inject edelim.

        readonly IMediator _mediator;

        public LandingPageController(IMediator mediator)
        {
            _mediator = mediator;
        }


        

        [HttpPost("CreateHomePageHeroBanner")]
        public async Task<IActionResult> CreateHomePageHeroBanner([FromForm]CreateHeroSectionRequest request)
        {
            CreateHeroSectionResponse response = await _mediator.Send(request);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response);
                case HttpStatusCode.BadRequest:
                    return BadRequest(response);
                case HttpStatusCode.NotFound:
                    return NotFound(response);
                default:
                    return StatusCode((int)response.StatusCode, response);
            }
        }

        [HttpGet("GetAllHomePageHeroBanner")]
        public async Task<IActionResult> GetAllHomePageHeroBanner([FromQuery]GetHeroSectionRequest request)
        {
            GetHeroSectionResponse response = await _mediator.Send(request);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response);
                case HttpStatusCode.BadRequest:
                    return BadRequest(response);
                case HttpStatusCode.NotFound:
                    return NotFound(response);
                default:
                    return StatusCode((int)response.StatusCode, response);
            }
        }

        [HttpPut("UpdateHomePageHeroBanner")]
        public async Task<IActionResult> UpdateHomePageHeroBanner([FromForm] UpdateHeroSectionRequest request)
        {
            UpdateHeroSectionResponse response = await _mediator.Send(request);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response);
                case HttpStatusCode.BadRequest:
                    return BadRequest(response);
                case HttpStatusCode.NotFound:
                    return NotFound(response);
                default:
                    return StatusCode((int)response.StatusCode, response);
            }
        }
        [HttpDelete("DeleteHomePageHeroBanner")]
        public async Task<IActionResult> DeleteHomePageHeroBanner([FromQuery] DeleteHeroSectionRequest request)
        {
            DeleteHeroSectionResponse response = await _mediator.Send(request);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response);
                case HttpStatusCode.BadRequest:
                    return BadRequest(response);
                case HttpStatusCode.NotFound:
                    return NotFound(response);
                default:
                    return StatusCode((int)response.StatusCode, response);
            }
        }
        



    }
}

