using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Abstractions.ProductImageFileCrud;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.RequestParamaters;
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
        readonly IProductImageFileReadRepository _productImageFileReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IStorageService _storageService;




        public ProductsController(
            IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            IProductImageFileReadRepository productImageFileReadRepository,
            IProductImageFileWriteRepository productImageFileWriteRepository,
            IStorageService storageService

            )
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _storageService = storageService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)
        {
            var product = _productReadRepository.GetAll();
            int totalProductCount = product.Count();  //Toplam ürün sayısı 
            int totalPageSize = (int)Math.Ceiling((double)totalProductCount / pagination.Size); //Toplam sayfa sayısı 

            var pagedProduct = product.Skip(pagination.Size * pagination.Page).Take(pagination.Size); //Pagination işlemleri
            int pagesize = pagedProduct.Count(); //Pagination işlemleri sonucu sayfada gösterilecek eleman sayısı
            bool hasNextPage = pagination.Page < totalPageSize - 1; //Bir sonraki sayfa var mı ? 
            bool hasPrevPage = pagination.Page > 0; //Bir önceki sayfa var mı ? 

            return Ok(new
            {
                TotalCount = totalProductCount,
                TotalPageSize = totalPageSize,
                CurrentPage = pagination.Page,
                HasNext = hasNextPage,
                HasPrev = hasPrevPage,
                Pagesize= pagesize,
                Products = pagedProduct
            });
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


        [HttpDelete("DeleteProductById")]
        public async Task<IActionResult> DeleteProduct([FromQuery(Name = "id")] string id)
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


        [HttpPost("[action]")]
        public async Task<IActionResult> Upload()
        {
            //Storage için global methodumuz.
            var result = await _storageService.UploadAsync("resource/product-images", Request.Form.Files);

            //Upload işlemi bittikten sonra , VeriTabanına kayıt işlemini gerçekleştirelim.
            //Birden fazla veri gelebilir şekilde projemizi tanımladığımız için ToList ile yollamamız gerekmekete. 

            await _productImageFileWriteRepository.AddRangeAsync(result.Select(d => new ProductImageFile()
            {
                FileName = d.fileName,
                Path = d.PathOrContainerName,
                Storage = _storageService.StorageType

            }).ToList()) ;

            await _productImageFileWriteRepository.SaveAsync();
            return Ok();

        }


    }
}









      