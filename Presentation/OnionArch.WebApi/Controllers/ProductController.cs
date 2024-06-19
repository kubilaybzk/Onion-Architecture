using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Features.Commands.Product.CreateOneProductWithImage;
using OnionArch.Application.Features.Commands.Product.DeleteProductById;
using OnionArch.Application.Features.Commands.Product.UpdateProductByIDCommands;
using OnionArch.Application.Features.Queries.Product.GetAllProducts;
using OnionArch.Application.Features.Queries.Product.GetLatesProducts;
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


        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet("GetLatesCreatedProducts")]

        public async Task<IActionResult> GetLatesCreatedProducts([FromQuery] GetLatesProductsRequest getLatesProductsRequest)
        {

            GetLatesProductsResponse ProductResponse = await _mediator.Send(getLatesProductsRequest);

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



        [HttpPut("UpdateProduct")]

        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductByIDCommandsRequest updateProductByIDCommandsRequest)
        {
            //Nasıl yollayacağımızı bulamadım normalde null gönderiyor ilerleyen aşamada düzeltilecek.
            updateProductByIDCommandsRequest.ImageFiles = Request.Form.Files;

            UpdateProductByIDCommandsResponse ProductResponse = await _mediator.Send(updateProductByIDCommandsRequest);

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









