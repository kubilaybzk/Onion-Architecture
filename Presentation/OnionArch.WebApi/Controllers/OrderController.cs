using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.DiscountCoupon.ApplyCouponToBasket;
using OnionArch.Application.Features.Commands.DiscountCoupon.CreateDiscountCoupon;
using OnionArch.Application.Features.Commands.DiscountCoupon.DeleteDiscountCoupon;
using OnionArch.Application.Features.Commands.DiscountCoupon.RemoveCouponFromBasket;
using OnionArch.Application.Features.Commands.DiscountCoupon.UpdateDiscountCoupon;
using OnionArch.Application.Features.Commands.OrderComands.CreateOrder;
using OnionArch.Application.Features.Commands.Payment.Complete3DPayment;
using OnionArch.Application.Features.Commands.Payment.CreatePayment;
using OnionArch.Application.Features.Queries.DiscountCoupon.GetAllDiscountCoupons;
using OnionArch.Application.Features.Queries.OrderQueries.GetOrderById;
using OnionArch.Application.Features.Queries.OrderQueries.GetUserOrders;
using OnionArch.Application.Features.Queries.PaymentQueries;
using OnionArch.Domain.Enums;
using System.Net;

namespace OnionArch.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
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

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOrderById([FromRoute] string id)
        {
            var request = new GetOrderByIdRequest { OrderId = id };
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

        [HttpGet]
        [Route("user-orders")]
        public async Task<IActionResult> GetUserOrders()
        {
            var request = new GetUserOrdersRequest();
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

        [HttpPut]
        [Route("status/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] string id, [FromBody] OrderStatus status)
        {
            // TODO: UpdateOrderStatus Command implementasyonu yapılabilir
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("cancel/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CancelOrder([FromRoute] string id)
        {
            // TODO: CancelOrder Command implementasyonu yapılabilir
            throw new NotImplementedException();
        }
    }
}
