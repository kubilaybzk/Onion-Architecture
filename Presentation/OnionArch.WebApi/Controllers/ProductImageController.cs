using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.ProductImageFileCrud;
using OnionArch.Application.Features.Queries.ProductImageFile;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Repositorys.ProductImageFileCrud;
using System;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnionArch.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        /* Bu bir eğitim videosu olduğundan dolayı burada mediator kullanmayı tercih etmedim. */ 

        private readonly IProductImageFileWriteRepository _IProductImageFileWriteRepository;
        private readonly IProductImageFileReadRepository _IProductImageFileReadRepository;

        public ImageController(IMediator mediator, IProductImageFileWriteRepository ıProductImageFileWriteRepository , IProductImageFileReadRepository ıProductImageFileReadRepository)
        {
            _IProductImageFileWriteRepository=ıProductImageFileWriteRepository;
            _IProductImageFileReadRepository = ıProductImageFileReadRepository;

        }

        [HttpGet("GetAllİmages")]
        public async Task<IActionResult> GetAllİmages()
        {

            var images = _IProductImageFileReadRepository.GetAll();
            var images2 = await images.ToListAsync();

            if(images2.Count > 0) {
                return Ok(images);
            }
            else
            {
                return NotFound();
            }

           

        }
        
        [HttpGet("GetAllİmagesById/{gelenid}")]
        public async Task<IActionResult> GetAllİmagesById([FromRoute] string gelenid)
        {
            try
            {

                if (!Guid.TryParse(gelenid, out Guid productIdGuid))
                {
                    return BadRequest("Gönderilen Id Formatı yanlış.");
                }

                //Ürün resimlerine bağlı olan ürünleri'de ekle.
               

                var query = await _IProductImageFileWriteRepository.Table
                    .Include(pif => pif.Products)
                    .Where(pif => pif.Products.Any(p => p.ID == productIdGuid))
                    .ToListAsync();

                //Gelen Ürünlerde id'ye göre filtre uygula.
                var data = query.Select(pif => new
                {
                    Id = pif.ID,
                    Showcase=pif.Showcase,
                    updateTime=pif.UpdateTime,
                    createTime=pif.CreateTime,
                    fileName =pif.FileName,
                    path = pif.Path,
                    storage = pif.Storage,


                }).ToList();

                 return Ok(data);

            }

            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }



        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [HttpGet("SelectShowCaseImage/{gelenid}")]
        public async Task<IActionResult> SelectShowCaseImage([FromRoute] string gelenid)
        {
            try
            {
                // Validate the input parameter
                if (!Guid.TryParse(gelenid, out Guid selectedImageId))
                {
                    return BadRequest("Invalid image ID format.");
                }

                // Find the selected image by ID
                var selectedImage = await _IProductImageFileWriteRepository.Table
                    .Include(p => p.Products)
                    .FirstOrDefaultAsync(pif => pif.ID == selectedImageId);

                if (selectedImage != null)
                {
                    // Get all product images related to the selected product
                    var allProductImages = _IProductImageFileWriteRepository.Table
                        .Where(pif => pif.Products.Any(p => p.ID == selectedImage.Products.First().ID));

                    // Set Showcase to false for all images except the selected one
                    foreach (var productImage in allProductImages)
                    {
                        productImage.Showcase = false;
                    }

                    // Set Showcase to true for the selected image
                    selectedImage.Showcase = true;

                    // Save changes to the database
                    await _IProductImageFileWriteRepository.SaveAsync();

                    return Ok();
                }

                // Handle the case where the specified ID is not found
                return NotFound();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }



    }
}
