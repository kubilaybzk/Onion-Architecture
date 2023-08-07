using System.Net;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Abstractions.CustomerCrud;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.View_Models;
using OnionArch.Domain.Entities;

namespace OnionArch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;


        readonly private ICustomerWriteRepository _customerWriteRepository;
        public ProductsController(
            IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            var product = _productReadRepository.GetAll();
            return Ok(product);
        }

        [HttpGet("GetSingleById")]
        public async Task<IActionResult> GetSingle(string id)
        {

            var product = await _productReadRepository.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpPost("CreateOneProduct")]
        public async Task<IActionResult> CreateOneProduct([FromQuery] VM_Create_Product product)
        {
            try
            {
                if (product is null)
                {
                    // Boş veri hatası için 400 Bad Request dönüşü ve özel hata mesajı
                    return BadRequest("Product data is missing.");
                }
                else
                {
                    // Veri eklenmesi işlemi
                    await _productWriteRepository.AddAsync(new Product
                    {
                        Price = product.Price,
                        Name = product.Name,
                        Stock = product.Stock
                    });
                    await _productWriteRepository.SaveAsync();

                    // 201 Created dönüşü ve eklenen ürün bilgisi
                    return StatusCode(201, product);
                }
            }
            catch (Exception ex)
            {
                // Genel bir hata mesajı dönüşü ve içerideki hata mesajı
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPut("UpdateProductById")]
        public async Task<IActionResult> UpdateProduct(VM_Update_Product model)
        {
            //Önclikle Update edilecek olan veriye ıd üzerinden erişelim.
            Product product = await _productReadRepository.GetByIdAsync(model.ID);
            //model'den gelen verileri bu verilere atayalım.
            product.Name = model.Name;
            product.Price = model.Price;
            product.Stock = model.Stock;
            //Değişiklik veri tabanına yansısın.
            await _productWriteRepository.SaveAsync();

            return Ok(product);
        }


        [HttpPut("DeleteProductById")]
        public async Task<IActionResult> DeleteProduct([FromRoute(Name = "id")] string id)
        {
            var result = await _productWriteRepository.RemoveAsync(id);

            switch (result)
            {
                case true:
                    await _productWriteRepository.SaveAsync();
                    return Ok(result);
                case false:
                    return NotFound();
                default:
                    return BadRequest(); // Eğer result farklı bir değerse, burada uygun bir IActionResult dönülmelidir.
            }
        }

    }
}









      