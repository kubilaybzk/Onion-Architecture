using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.Product.CreateOneProductWithImage;
using OnionArch.Application.Features.Commands.Product.DeleteProductById;
using OnionArch.Application.Features.Commands.Product.UpdateOneProduct;
using OnionArch.Application.Features.Queries.Product.GetAllProducts;
using OnionArch.Application.Features.Queries.Product.GetProductByCategory;
using OnionArch.Application.Features.Queries.Product.GetSingleById;
using OnionArch.Application.Features.Queries.Product.Product.GetAllProducts;
using System.Net;

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


        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> Get([FromQuery] GetAllProductsQueryRequest getAllProductsQueryRequest)
        {
            GetAllProductsQueryResponse ProductResponse = await _mediator.Send(getAllProductsQueryRequest);

            switch (ProductResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(ProductResponse);
                case HttpStatusCode.BadRequest:
                    return BadRequest(ProductResponse);
                case HttpStatusCode.NotFound:
                    return NotFound(ProductResponse);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)ProductResponse.StatusCode, ProductResponse);
            }
        }

        [AllowAnonymous]
        [HttpGet("GetProductByCategory")]
        public async Task<IActionResult> GetProductByCategory([FromQuery] GetProductByCategoryRequest getProductByCategoryRequest)
        {
            GetProductByCategoryResponse ProductResponse = await _mediator.Send(getProductByCategoryRequest);

            switch (ProductResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(ProductResponse);
                case HttpStatusCode.BadRequest:
                    return BadRequest(ProductResponse);
                case HttpStatusCode.NotFound:
                    return NotFound(ProductResponse);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)ProductResponse.StatusCode, ProductResponse);
            }
        }

        [AllowAnonymous]
        [HttpGet("GetSingleById/{id}")]
        public async Task<IActionResult> GetSingle([FromRoute] GetSingleByIdQueryRequest getSingleByIdQueryRequest)
        {
            GetSingleByIdQueryResponse ProductResponse = await _mediator.Send(getSingleByIdQueryRequest);

            switch (ProductResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(ProductResponse);
                case HttpStatusCode.BadRequest:
                    return BadRequest(ProductResponse);
                case HttpStatusCode.NotFound:
                    return NotFound(ProductResponse);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)ProductResponse.StatusCode, ProductResponse);
            }
        }


        [HttpPut("UpdateProductById")]
        public async Task<IActionResult> UpdateProduct(UpdateOneProductRequest updateOneProductRequest)
        {
            UpdateOneProductResponse ProductResponse = await _mediator.Send(updateOneProductRequest);

            switch (ProductResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(ProductResponse);
                case HttpStatusCode.BadRequest:
                    return BadRequest(ProductResponse);
                case HttpStatusCode.NotFound:
                    return NotFound(ProductResponse);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)ProductResponse.StatusCode, ProductResponse);
            }
        }


        [HttpDelete("DeleteProductById")]
        public async Task<IActionResult> DeleteProduct([FromQuery] DeleteProductByIdCommandsRequest deleteProductByIdCommandsRequest)
        {
            DeleteProductByIdCommandsResponse ProductResponse = await _mediator.Send(deleteProductByIdCommandsRequest);

            switch (ProductResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(ProductResponse);
                case HttpStatusCode.BadRequest:
                    return BadRequest(ProductResponse);
                case HttpStatusCode.NotFound:
                    return NotFound(ProductResponse);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)ProductResponse.StatusCode, ProductResponse);
            }
        }

        [HttpPost("CreateOneProductWithImage")]

        public async Task<IActionResult> CreateOneProductWithImage([FromForm] CreateOneProductWithImageRequest createOneProductWithImageRequest)
        {
            //Nasıl yollayacağımızı bulamadım normalde null gönderiyor ilerleyen aşamada düzeltilecek.
            createOneProductWithImageRequest.ImageFiles = Request.Form.Files;

            CreateOneProductWithImageResponse ProductResponse = await _mediator.Send(createOneProductWithImageRequest);

            switch (ProductResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(ProductResponse);
                case HttpStatusCode.BadRequest:
                    return BadRequest(ProductResponse);
                case HttpStatusCode.NotFound:
                    return NotFound(ProductResponse);
                // Diğer durumlar için gereken kodları buraya ekleyebilirsin
                default:
                    return StatusCode((int)ProductResponse.StatusCode, ProductResponse);
            }
        }



    }
}









