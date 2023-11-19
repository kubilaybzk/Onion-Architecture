using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Queries.BackEndLogs;
using OnionArch.Application.Features.Queries.Product.GetAllProducts;
using OnionArch.Application.Features.Queries.Product.Product.GetAllProducts;

namespace OnionArch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class BackEndLogsController : ControllerBase
    {

        readonly IMediator _mediator;

        public BackEndLogsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllLogs([FromQuery] BackEndLogsQueryRequest backEndLogsQueryRequest)
        {
            BackEndLogsQueryResponse backendlogresponce = await _mediator.Send(backEndLogsQueryRequest);
            if (backendlogresponce.StatusCode == StatusCodes.Status200OK)
            {
                // Başarılı güncelleme durumunda 200 OK kodunu dönün
                return Ok(backendlogresponce);
            }
            else if (backendlogresponce.StatusCode == StatusCodes.Status404NotFound)
            {
                // Ürün bulunamadı durumunda 404 Not Found kodunu dönün
                return NotFound(backendlogresponce);
            }
            else if (backendlogresponce.StatusCode == StatusCodes.Status500InternalServerError)
            {
                // İç sunucu hatası durumunda 500 Internal Server Error kodunu dönün
                return StatusCode(StatusCodes.Status500InternalServerError, backendlogresponce);
            }
            else
            {
                // Diğer durumlar için varsayılan bir hata kodu dönün
                return BadRequest(backendlogresponce);
            }
        }

       


    }
}









