using P = OnionArch.Domain.Entities;
using MediatR;
using OnionArch.Application.Abstractions.ProductCrud;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http; // HTTP durum kodları için ekledik
using Microsoft.Extensions.Logging; // Hata günlüğü için ekledik
using System;
using OnionArch.Domain.Entities;

namespace OnionArch.Application.Features.Commands.Product.UpdateOneProduct
{
    public class UpdateOneProductHandler : IRequestHandler<UpdateOneProductRequest, UpdateOneProductResponse>
    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;
       

        public UpdateOneProductHandler(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, ILogger<UpdateOneProductHandler> logger)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
          
        }

        public async Task<UpdateOneProductResponse> Handle(UpdateOneProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                P.Product product = await _productReadRepository.GetByIdAsync(request.ID);

                if (product == null)
                {
                    // Ürün bulunamadıysa 404 Not Found durum kodunu döndürebilirsiniz.
                    return new UpdateOneProductResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Ürün bulunamadı."
                    };
                }

                // Model'den gelen verileri bu verilere atayalım.
                product.Name = request.Name;
                product.Price = request.Price;
                product.Stock = request.Stock;

                // Değişiklik veri tabanına yansısın.
                await _productWriteRepository.SaveAsync();
                var productImages = product.ProductImageFiles
                   .Select(p => new
                   {
                       p.Path,
                       p.FileName,
                       p.ID,
                       p.CreateTime,
                       p.UpdateTime
                   }).ToList();
                var productResult = new
                {
                    product.Name,
                    product.Price,
                    product.Stock,
                    product.ID,
                    productImages,
                    product.CreateTime,
                    product.UpdateTime
                };

                return new UpdateOneProductResponse
                {
                    Product = productResult,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Ürün güncellendi"
                };

            }
            catch (Exception ex)
            {
                // Hata durumunda 500 Internal Server Error durum kodunu döndürebilirsiniz.
                return new UpdateOneProductResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = ex.ToString()
                };
            }
        }
    }
}