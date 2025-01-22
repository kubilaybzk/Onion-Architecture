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
using System.Web;

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
        [AllowAnonymous]
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

         
        [AllowAnonymous]
        [HttpPost]
        [Route("complete-3dv2")]
        public async Task<IActionResult> Complete3DPaymentv2()
        {
            try
            {
                // Tüm form verilerini loglayalım (debug için)
                foreach (var key in Request.Form.Keys)
                {
                   Console.WriteLine(($"{key}: {Request.Form[key]}"));
                }

                // Form verilerini alalım
                var status = Request.Form["status"].ToString();
                var paymentId = Request.Form["paymentId"].ToString();
                var conversationId = Request.Form["conversationId"].ToString();
                var conversationData = Request.Form["conversationData"].ToString();
                var mdStatus = Request.Form["mdStatus"].ToString();
                var signature = Request.Form["signature"].ToString();

                // Başarı kontrolü
                if (mdStatus != "1" || status != "success")
                {
                    return BadRequest(new Complete3DResponse
                    {
                        PaymentSuccess = false,
                        Message = "3D doğrulama başarısız",
                        ErrorMessage = "Banka doğrulaması başarısız oldu",
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        HassError = true
                    });
                }

                // Ödemeyi tamamla
                var response = await _mediator.Send(new Complete3DRequest
                {
                    PaymentId = paymentId,
                    ConversationId= conversationId,
                    ConversationData=conversationData,
                    Status = status,
                    Signature = signature
                });

                if (response.PaymentSuccess)
                {
                    return Ok(new Complete3DResponse
                    {
                        PaymentSuccess = true,
                        TransactionId = response.TransactionId,
                        Message = "Ödeme başarıyla tamamlandı",
                        StatusCode = HttpStatusCode.OK,
                        HassError = false
                    });
                }
                else
                {
                    return BadRequest(new Complete3DResponse
                    {
                        PaymentSuccess = false,
                        Message = "Ödeme işlemi başarısız",
                        ErrorMessage = response.ErrorMessage,
                        StatusCode = HttpStatusCode.BadRequest,
                        HassError = true
                    });
                }
            }
            catch (Exception ex)
            {
                
                return BadRequest(new Complete3DResponse
                {
                    PaymentSuccess = false,
                    Message = "3D doğrulama işlemi başarısız",
                    ErrorMessage = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest,
                    HassError = true
                });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("complete-3d")]
        public async Task<IActionResult> Complete3DPayment()
        {
            try
            {
                // Form verilerini al
                var status = Request.Form["status"].ToString();
                var paymentId = Request.Form["paymentId"].ToString();
                var conversationId = Request.Form["conversationId"].ToString();
                var conversationData = Request.Form["conversationData"].ToString();
                var mdStatus = Request.Form["mdStatus"].ToString();
                var signature = Request.Form["signature"].ToString();

                // Başarı kontrolü
                if (mdStatus != "1" || status != "success")
                {
                    // HTML response with error
                    var errorHtml = GetResultHtml(
                        success: false,
                        message: "3D doğrulama başarısız",
                        errorMessage: "Banka doğrulaması başarısız oldu"
                    );
                    return Content(errorHtml, "text/html");
                }

                // Ödemeyi tamamla
                var response = await _mediator.Send(new Complete3DRequest
                {
                    PaymentId = paymentId,
                    ConversationId = conversationId,
                    ConversationData = conversationData,
                    Status = status,
                    Signature = signature
                });

                if (response.PaymentSuccess)
                {
                    // HTML response with success
                    var successHtml = GetResultHtml(
                        success: true,
                        message: "Ödeme başarıyla tamamlandı",
                        transactionId: response.TransactionId
                    );
                    return Content(successHtml, "text/html");
                }
                else
                {
                    // HTML response with payment error
                    var errorHtml = GetResultHtml(
                        success: false,
                        message: "Ödeme işlemi başarısız",
                        errorMessage: response.ErrorMessage
                    );
                    return Content(errorHtml, "text/html");
                }
            }
            catch (Exception ex)
            {
                // HTML response with exception
                var errorHtml = GetResultHtml(
                    success: false,
                    message: "3D doğrulama işlemi başarısız",
                    errorMessage: ex.Message
                );
                return Content(errorHtml, "text/html");
            }
        }

        private string GetResultHtml(bool success, string message, string errorMessage = null, string transactionId = null)
        {
            var template = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <script src=\"https://cdn.tailwindcss.com\"></script>\r\n</head>\r\n<body>\r\n    <!-- container -->\r\n    <div class=\"min-h-screen bg-gray-50 flex items-center justify-center p-4\">\r\n        <!-- card -->\r\n        <div class=\"bg-white rounded-lg shadow-lg p-8 max-w-md w-full mx-auto\">\r\n            <!-- success state -->\r\n            [SUCCESS_CONTENT]\r\n            <!-- error state -->\r\n            [ERROR_CONTENT]\r\n        </div>\r\n    </div>\r\n\r\n    <script>\r\n        [SCRIPT_CONTENT]\r\n    </script>\r\n</body>\r\n</html>"; // Ana HTML template'i buraya yükleyin

            // Başarılı durum için içerik
            var successContent = @"
        <div class='text-center'>
            <div class='mx-auto flex items-center justify-center h-16 w-16 rounded-full bg-green-100 mb-4'>
                <svg class='h-10 w-10 text-green-500' fill='none' stroke='currentColor' viewBox='0 0 24 24'>
                    <path stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M5 13l4 4L19 7'></path>
                </svg>
            </div>
            <h2 class='text-2xl font-semibold text-gray-800 mb-2'>Ödeme Başarılı</h2>
            <p class='text-gray-600 mb-4'>" + HttpUtility.HtmlEncode(message) + @"</p>" +
                    (string.IsNullOrEmpty(transactionId) ? "" : @"
            <div class='bg-gray-50 rounded-md p-4 mt-4'>
                <p class='text-sm text-gray-500'>İşlem No</p>
                <p class='text-gray-700 font-medium'>" + HttpUtility.HtmlEncode(transactionId) + @"</p>
            </div>") + @"
            
        </div>";

            // Hata durumu için içerik
            var errorContent = @"
        <div class='text-center'>
            <div class='mx-auto flex items-center justify-center h-16 w-16 rounded-full bg-red-100 mb-4'>
                <svg class='h-10 w-10 text-red-500' fill='none' stroke='currentColor' viewBox='0 0 24 24'>
                    <path stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M6 18L18 6M6 6l12 12'></path>
                </svg>
            </div>
            <h2 class='text-2xl font-semibold text-gray-800 mb-2'>Ödeme Başarısız</h2>
            <p class='text-gray-600 mb-4'>" + HttpUtility.HtmlEncode(message) + @"</p>" +
                    (!string.IsNullOrEmpty(errorMessage) ? @"
            <div class='bg-red-50 rounded-md p-4 mt-4'>
                <p class='text-sm text-red-500'>Hata Detayı</p>
                <p class='text-gray-700'>" + HttpUtility.HtmlEncode(errorMessage) + @"</p>
            </div>" : "") + @"
            
        </div>";

            // Script içeriği
            var scriptContent = success
                ? "window.parent.postMessage({ type: 'paymentSuccess', transactionId: '" + HttpUtility.JavaScriptStringEncode(transactionId ?? "") + "' }, '*');"
                : "window.parent.postMessage({ type: 'paymentError', error: '" + HttpUtility.JavaScriptStringEncode(errorMessage ?? "") + "' }, '*');";

            // Template içeriklerini yerleştir
            template = template
                .Replace("[SUCCESS_CONTENT]", success ? successContent : "")
                .Replace("[ERROR_CONTENT]", !success ? errorContent : "")
                .Replace("[SCRIPT_CONTENT]", scriptContent);

            return template;
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
