using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.FileCrud;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Abstractions.ProductImageFileCrud;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Features.Queries.ProductImageFile;
using OnionArch.Application.Repositories.CategoryImageFileCrud;
using OnionArch.Application.View_Models.Category;
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
        private readonly IStorageService _IStorageService;
        private readonly IFileWriteRepository _IFileWriteRepository;
        private readonly IFileReadRepository _IFileReadRepository;
        private readonly ICategoryImageFileReadRepository _CategoryImageFileReadRepository;
        private readonly ICategoryImageFileWriteRepository _CategoryImageFileWriteRepository;
        private readonly IProductReadRepository _ProductReadRepository;
        private readonly IProductWriteRepository _ProductWriteRepository;

        public ImageController(IMediator mediator, IProductImageFileWriteRepository ıProductImageFileWriteRepository, IProductImageFileReadRepository ıProductImageFileReadRepository, IStorageService ıStorageService, IFileWriteRepository ıFileWriteRepository, IFileReadRepository ıFileReadRepository, ICategoryImageFileReadRepository categoryImageFileReadRepository, ICategoryImageFileWriteRepository categoryImageFileWriteRepository, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _IProductImageFileWriteRepository = ıProductImageFileWriteRepository;
            _IProductImageFileReadRepository = ıProductImageFileReadRepository;
            _IStorageService = ıStorageService;
            _IFileWriteRepository = ıFileWriteRepository;
            _IFileReadRepository = ıFileReadRepository;
            _CategoryImageFileReadRepository = categoryImageFileReadRepository;
            _CategoryImageFileWriteRepository = categoryImageFileWriteRepository;
            _ProductReadRepository = productReadRepository;
            _ProductWriteRepository = productWriteRepository;
        }





        [Authorize(AuthenticationSchemes = "Admin")]
        [HttpPost("SelectShowCaseImage")]
        public async Task<IActionResult> SelectShowCaseImage([FromQuery] string gelenid)
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


        [HttpDelete("DeleteProductImage")]
        public async Task<IActionResult> DeleteProductImage([FromQuery] string fileName ,string ProductImageId)
        {
            var CurrentImages =  _IStorageService.HasFile(fileName, "product-images");
  

            if (CurrentImages)
            {
                var deleteProductsImage = await _IProductImageFileWriteRepository.RemoveAsync(ProductImageId);
                await _IStorageService.DeleteFileAsync(fileName, "wwwroot/resource/product-images");
                await _IProductImageFileWriteRepository.SaveAsync();
                return Ok();
            }
            else return NotFound();
        }
        


    }
}
