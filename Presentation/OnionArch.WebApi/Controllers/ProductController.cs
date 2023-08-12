using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OnionArch.Application.Abstractions.CustomerCrud;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.RequestParamaters;
using OnionArch.Application.View_Models;
using OnionArch.Domain.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace OnionArch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(
            IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
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
            //Burada iki farklı path'i bileştirme işlemi yapıyruz.
                // wwwroot özel bir dosya sistemi bundan dolayı burada göresellerimizi depolamak istiyoruz.
                    //Burada şimdi wwwroot'un pat bilgisini alıp içinde nasıl bir mantıkla bir dosyalama sistemi olacak onu düzenleyelim.
                        
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "resource/product-images");
            //Yani gelen dosyalar wwwroot/resource/product-images altında olsun demek istedik üst satırda.

            //path var mı yok mu kontrol ediyoruz.
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            //Burada bu path içinde birden fazla veri olabilir,
              //Bundan dolayı burada biz bir döngü içinde dönüş yapacağız bu arada gelen görselin isim değerini
              
                    
            Random r = new();
            foreach (IFormFile file in Request.Form.Files)
            {
                //Şimdilik biz kendimiz belirlemek isteyelim  yani bu şu anlama gelecek gelen resim random bir sayı olsun  .

                string fullPath = Path.Combine(uploadPath, $"{r.Next()}{Path.GetExtension(file.FileName)}");

                //Yani gelen dosyalar wwwroot/ resource / product - images altında olsun demek istedik üst satırda.

                using FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                //Gelen veriyi almak için konfigürasyonlar düzenledik.
                await file.CopyToAsync(fileStream);
                //Gelen veriyi kopyaladık.
                await fileStream.FlushAsync();
                //Bütün kopyalama işleminde oluşan verileri ortadan kaldırdık.
            }
            return Ok();
        }
    }
}









      