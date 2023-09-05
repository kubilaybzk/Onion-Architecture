using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.AppUser.CreateUser;
using OnionArch.Application.Features.Commands.AppUser.LoginUser;
using OnionArch.Application.Features.Commands.AppUser.LoginUser.FacebookLogin;
using OnionArch.Application.Features.Commands.AppUser.LoginUser.GoogleLogin;

namespace OnionArch.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //Her şeyden önce ilk olarak mediator nesnemizi burada oluşturalım ve inject edelim.

        readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        //Bir tane kullanıcı oluşturma action fonksiyonu oluşturalım.

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody]CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse response= await _mediator.Send(createUserCommandRequest);
            return Ok(response);
        }




        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommandsRequest loginUserCommandsRequest)
        {
            LoginUserCommandsResponse response = await _mediator.Send(loginUserCommandsRequest);
            return Ok(response);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest googleLoginCommandRequest)
        {
            GoogleLoginCommandResponse response = (GoogleLoginCommandResponse)await _mediator.Send(googleLoginCommandRequest);
            return Ok(response);
        }

        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin(FacebookLoginCommandRequest facebookLoginCommandRequest)
        {
            FacebookLoginCommandResponse response = await _mediator.Send(facebookLoginCommandRequest);
            return Ok(response);
        }


    }
}

