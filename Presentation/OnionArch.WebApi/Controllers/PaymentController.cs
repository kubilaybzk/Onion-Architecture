using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.DiscountCoupon.ApplyCouponToBasket;
using OnionArch.Application.Features.Commands.DiscountCoupon.CreateDiscountCoupon;
using OnionArch.Application.Features.Commands.DiscountCoupon.DeleteDiscountCoupon;
using OnionArch.Application.Features.Commands.DiscountCoupon.RemoveCouponFromBasket;
using OnionArch.Application.Features.Commands.DiscountCoupon.UpdateDiscountCoupon;
using OnionArch.Application.Features.Commands.Payment.Complete3DPayment;
using OnionArch.Application.Features.Commands.Payment.CreatePayment;
using OnionArch.Application.Features.Queries.DiscountCoupon.GetAllDiscountCoupons;
using OnionArch.Application.Features.Queries.PaymentQueries;
using System.Net;

namespace OnionArch.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
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

        [HttpPost]
        [Route("complete-3d")]
        public async Task<IActionResult> Complete3DPayment([FromBody] Complete3DRequest request)
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
        [Route("history")]
        public async Task<IActionResult> GetPaymentHistory([FromQuery] string orderId = null)
        {
            var request = new GetPaymentHistoryRequest { OrderId = orderId };
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

        // İptal/İade endpointleri eklenecek
        [HttpPost]
        [Route("cancel/{transactionId}")]
        [Authorize(Roles = "Admin")] // Sadece admin iptal edebilir
        public async Task<IActionResult> CancelPayment([FromRoute] string transactionId)
        {
            // TODO: Cancel Payment Command implementasyonu
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("refund/{transactionId}")]
        [Authorize(Roles = "Admin")] // Sadece admin iade yapabilir
        public async Task<IActionResult> RefundPayment([FromRoute] string transactionId, [FromBody] decimal amount)
        {
            // TODO: Refund Payment Command implementasyonu
            throw new NotImplementedException();
        }
    }
}
