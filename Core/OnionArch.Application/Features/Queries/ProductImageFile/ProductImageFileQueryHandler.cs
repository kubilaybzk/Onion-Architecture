using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnionArch.Application.Abstractions.ProductImageFileCrud;
using OnionArch.Application.Features.Queries.Product.GetAllProducts;
using OnionArch.Application.Features.Queries.Product.Product.GetAllProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.ProductImageFile
{
    public class ProductImageFileQueryHandler : IRequestHandler<ProductImageFileQueryRequest, ProductImageFileQueryResponse>
    {

        private readonly IProductImageFileWriteRepository _IProductImageFileWriteRepository;
        private readonly IProductImageFileReadRepository _IProductImageFileReadRepository;
        private readonly ILogger<ProductImageFileQueryHandler> _logger;

        public ProductImageFileQueryHandler (IProductImageFileWriteRepository ıProductImageFileWriteRepository, IProductImageFileReadRepository ıProductImageFileReadRepository, ILogger<ProductImageFileQueryHandler> logger)
        {
            _IProductImageFileWriteRepository = ıProductImageFileWriteRepository;
            _IProductImageFileReadRepository = ıProductImageFileReadRepository;
            _logger = logger;
        }

        public async Task<ProductImageFileQueryResponse> Handle(ProductImageFileQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var images = _IProductImageFileReadRepository.GetAll();
                var images2=await images.ToListAsync();
                return new ProductImageFileQueryResponse(){
                    StatusCode= StatusCodes.Status200OK,
                    Images = images,
                    Message="Görseller bulundu yeyy !! "
                    

                   
                };

            }
            catch (Exception ex) { 
            return new ProductImageFileQueryResponse() { Message = ex.Message, StatusCode=StatusCodes.Status500InternalServerError };
            }
        }
    }
}
