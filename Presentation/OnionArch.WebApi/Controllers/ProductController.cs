using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.Product.CreateOneProductNoImage;
using OnionArch.Application.Features.Commands.Product.CreateOneProductWithImage;
using OnionArch.Application.Features.Commands.Product.DeleteProductById;
using OnionArch.Application.Features.Commands.Product.UpdateOneProduct;
using OnionArch.Application.Features.Queries.Product.GetAllProducts;
using OnionArch.Application.Features.Queries.Product.GetSingleById;
using OnionArch.Application.Features.Queries.Product.Product.GetAllProducts;

namespace OnionArch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ProductsController : ControllerBase
    {

        readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get([FromQuery] GetAllProductsQueryRequest getAllProductsQueryRequest)
        {
            GetAllProductsQueryResponse productResponse = await _mediator.Send(getAllProductsQueryRequest);
            if (productResponse.StatusCode == StatusCodes.Status200OK)
            {
                // Başarılı güncelleme durumunda 200 OK kodunu dönün
                return Ok(productResponse);
            }
            else if (productResponse.StatusCode == StatusCodes.Status404NotFound)
            {
                // Ürün bulunamadı durumunda 404 Not Found kodunu dönün
                return NotFound(productResponse);
            }
            else if (productResponse.StatusCode == StatusCodes.Status500InternalServerError)
            {
                // İç sunucu hatası durumunda 500 Internal Server Error kodunu dönün
                return StatusCode(StatusCodes.Status500InternalServerError, productResponse);
            }
            else
            {
                // Diğer durumlar için varsayılan bir hata kodu dönün
                return BadRequest(productResponse);
            }
        }


        [HttpGet("GetSingleById/{id}")]
        public async Task<IActionResult> GetSingle([FromRoute] GetSingleByIdQueryRequest getSingleByIdQueryRequest)
        {
            GetSingleByIdQueryResponse productResponse = await _mediator.Send(getSingleByIdQueryRequest);
            if (productResponse.StatusCode == StatusCodes.Status200OK)
            {
                // Başarılı güncelleme durumunda 200 OK kodunu dönün
                return Ok(productResponse);
            }
            else if (productResponse.StatusCode == StatusCodes.Status404NotFound)
            {
                // Ürün bulunamadı durumunda 404 Not Found kodunu dönün
                return NotFound(productResponse);
            }
            else if (productResponse.StatusCode == StatusCodes.Status500InternalServerError)
            {
                // İç sunucu hatası durumunda 500 Internal Server Error kodunu dönün
                return StatusCode(StatusCodes.Status500InternalServerError, productResponse);
            }
            else
            {
                // Diğer durumlar için varsayılan bir hata kodu dönün
                return BadRequest(productResponse);
            }
        }

        
        [HttpPost("CreateOneProduct")]
        
        public async Task<IActionResult> CreateOneProduct([FromQuery] CreateOneProductNoImageRequest createOneProductNoImageRequest)
        {
            CreateOneProductNoImageResponse productResponse = await _mediator.Send(createOneProductNoImageRequest);
            if (productResponse.StatusCode == StatusCodes.Status200OK)
            {
                // Başarılı güncelleme durumunda 200 OK kodunu dönün
                return Ok(productResponse);
            }
            else if (productResponse.StatusCode == StatusCodes.Status404NotFound)
            {
                // Ürün bulunamadı durumunda 404 Not Found kodunu dönün
                return NotFound(productResponse);
            }
            else if (productResponse.StatusCode == StatusCodes.Status500InternalServerError)
            {
                // İç sunucu hatası durumunda 500 Internal Server Error kodunu dönün
                return StatusCode(StatusCodes.Status500InternalServerError, productResponse);
            }
            else
            {
                // Diğer durumlar için varsayılan bir hata kodu dönün
                return BadRequest(productResponse);
            }

        }


        [HttpPut("UpdateProductById")]
        public async Task<IActionResult> UpdateProduct(UpdateOneProductRequest updateOneProductRequest)
        {
            UpdateOneProductResponse productResponse = await _mediator.Send(updateOneProductRequest);

            if (productResponse.StatusCode == StatusCodes.Status200OK)
            {
                // Başarılı güncelleme durumunda 200 OK kodunu dönün
                return Ok(productResponse);
            }
            else if (productResponse.StatusCode == StatusCodes.Status404NotFound)
            {
                // Ürün bulunamadı durumunda 404 Not Found kodunu dönün
                return NotFound(productResponse);
            }
            else if (productResponse.StatusCode == StatusCodes.Status500InternalServerError)
            {
                // İç sunucu hatası durumunda 500 Internal Server Error kodunu dönün
                return StatusCode(StatusCodes.Status500InternalServerError, productResponse);
            }
            else
            {
                // Diğer durumlar için varsayılan bir hata kodu dönün
                return BadRequest(productResponse);
            }
        }


        [HttpDelete("DeleteProductById")]
        public async Task<IActionResult> DeleteProduct([FromQuery] DeleteProductByIdCommandsRequest deleteProductByIdCommandsRequest)
        {
            DeleteProductByIdCommandsResponse productResponse = await _mediator.Send(deleteProductByIdCommandsRequest);

            if (productResponse.StatusCode == StatusCodes.Status200OK)
            {
                // Başarılı güncelleme durumunda 200 OK kodunu dönün
                return Ok(productResponse);
            }
            else if (productResponse.StatusCode == StatusCodes.Status404NotFound)
            {
                // Ürün bulunamadı durumunda 404 Not Found kodunu dönün
                return NotFound(productResponse);
            }
            else if (productResponse.StatusCode == StatusCodes.Status500InternalServerError)
            {
                // İç sunucu hatası durumunda 500 Internal Server Error kodunu dönün
                return StatusCode(StatusCodes.Status500InternalServerError, productResponse);
            }
            else
            {
                // Diğer durumlar için varsayılan bir hata kodu dönün
                return BadRequest(productResponse);
            }
        }


        [HttpPost("CreateOneProductWithImage")]

        public async Task<IActionResult> CreateOneProductWithImage([FromForm] CreateOneProductWithImageRequest createOneProductWithImageRequest)
        {
            //Nasıl yollayacağımızı bulamadım normalde null gönderiyor ilerleyen aşamada düzeltilecek.
            createOneProductWithImageRequest.ImageFiles = Request.Form.Files;

            CreateOneProductWithImageResponse productResponse = await _mediator.Send(createOneProductWithImageRequest);
            if (productResponse.StatusCode == StatusCodes.Status200OK)
            {
                // Başarılı güncelleme durumunda 200 OK kodunu dönün
                return Ok(productResponse);
            }
            else if (productResponse.StatusCode == StatusCodes.Status404NotFound)
            {
                // Ürün bulunamadı durumunda 404 Not Found kodunu dönün
                return NotFound(productResponse);
            }
            else if (productResponse.StatusCode == StatusCodes.Status500InternalServerError)
            {
                // İç sunucu hatası durumunda 500 Internal Server Error kodunu dönün
                return StatusCode(StatusCodes.Status500InternalServerError, productResponse);
            }
            else
            {
                // Diğer durumlar için varsayılan bir hata kodu dönün
                return BadRequest(productResponse);
            }
        }



    }
}









