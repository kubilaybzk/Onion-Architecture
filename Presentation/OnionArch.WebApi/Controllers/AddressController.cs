using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.AddressCommands.CreateAddressCommands;
using OnionArch.Application.Features.Commands.AddressCommands.DeleteAddressCommand;
using OnionArch.Application.Features.Commands.AddressCommands.UpdateAddressCommand;
using OnionArch.Application.Features.Queries.Address.GetAddress;
using OnionArch.Application.Features.Queries.BackEndLogs;

namespace OnionArch.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class AddressController : ControllerBase
    {
        readonly IMediator _mediator;

        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("CreateAddress")]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressCommandRequest request)
        {
           var result = await _mediator.Send(request);
            return Ok(result);
           
        }

        [HttpDelete("DeleteAddress")]
        public async Task<IActionResult> DeleteAddress([FromQuery] DeleteAddressCommandRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("GetAddresses")]
        public async Task<IActionResult> GetAddresses([FromRoute] GetAddressRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }


        [HttpPut("UpdateAddress")]
        public async Task<IActionResult> UpdateAddress([FromForm] UpdateAddressCommandRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

    }
}
