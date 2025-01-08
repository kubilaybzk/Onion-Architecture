using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.DiscountCoupon.ApplyCouponToBasket;
using OnionArch.Application.Features.Commands.DiscountCoupon.CreateDiscountCoupon;
using OnionArch.Application.Features.Commands.DiscountCoupon.DeleteDiscountCoupon;
using OnionArch.Application.Features.Commands.DiscountCoupon.RemoveCouponFromBasket;
using OnionArch.Application.Features.Commands.DiscountCoupon.UpdateDiscountCoupon;
using OnionArch.Application.Features.Queries.DiscountCoupon.GetAllDiscountCoupons;
using System.Net;

namespace OnionArch.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class CuponsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CuponsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("ApplyCoupon/{code}")]
        public async Task<IActionResult> ApplyCoupon([FromRoute] string code)
        {
            var request = new ApplyCouponToBasketCommandRequest { CouponCode = code };
            var response = await _mediator.Send(request);

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

        [AllowAnonymous]
        [HttpDelete("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon()
        {
            var request = new RemoveCouponFromBasketCommandRequest { };
            var response = await _mediator.Send(request);

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

        [AllowAnonymous]
        [HttpPost("CreateCoupon")]
        public async Task<IActionResult> Create([FromForm] CreateDiscountCouponCommandRequest request)
        {
            var response = await _mediator.Send(request);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    return StatusCode((int)response.StatusCode, response);
                case System.Net.HttpStatusCode.BadRequest:
                    return BadRequest(response);
                default:
                    return StatusCode((int)response.StatusCode, response);
            }
        }

        [AllowAnonymous]
        [HttpPut("UpdateCoupon")]
        public async Task<IActionResult> UpdateCoupon([FromForm] UpdateDiscountCouponCommandRequest request)
        {
             
            var response = await _mediator.Send(request);

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

        [AllowAnonymous]
        [HttpDelete("DeleteCoupon/{id}")]
        public async Task<IActionResult> DeleteCoupon([FromRoute] Guid id)
        {
            var request = new DeleteDiscountCouponCommandRequest { Id = id };
            var response = await _mediator.Send(request);

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
        [AllowAnonymous]
        [HttpGet("GetAllCouponList")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllDiscountCouponsQueryRequest request)
        {

            var response = await _mediator.Send(request);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return Ok(response);
                case System.Net.HttpStatusCode.BadRequest:
                    return BadRequest(response);
                default:
                    return StatusCode((int)response.StatusCode, response);
            }
        }
    }
}
