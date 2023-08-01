using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Abstractions.CustomerCrud;
using OnionArch.Application.Abstractions.ProductCrud;
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var product = _productReadRepository.GetAll();
            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(string id)
        {

            var product = await _productReadRepository.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOneBook([FromBody] Product book)
        {
            try
            {
                if (book is null)
                {
                    return BadRequest();
                }
                else
                {
                    await  _productWriteRepository.AddAsync(book);
                    await _productWriteRepository.SaveAsync();
                    return StatusCode(201, book);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }


    }
}