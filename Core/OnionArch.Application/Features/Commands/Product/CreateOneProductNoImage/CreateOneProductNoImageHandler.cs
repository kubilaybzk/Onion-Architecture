using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.ProductCrud;
using p=OnionArch.Domain.Entities;

namespace OnionArch.Application.Features.Commands.Product.CreateOneProductNoImage
{
    


    public class CreateOneProductNoImageHandler : IRequestHandler<CreateOneProductNoImageRequest, CreateOneProductNoImageResponse>
    {
        readonly private IProductWriteRepository _productWriteRepository;

        public CreateOneProductNoImageHandler(IProductWriteRepository productWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
        }

        public async Task<CreateOneProductNoImageResponse> Handle(CreateOneProductNoImageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.product is null)
                {
                    // Boş veri hatası için 400 Bad Request dönüşü ve özel hata mesajı
                    return new CreateOneProductNoImageResponse
                    {
                        StatusCode = StatusCodes.Status204NoContent,
                        Message = "Ürün bulunamadı."
                    };
                }
                else
                {
                    // Veri eklenmesi işlemi

                    var NewProduct = new p.Product
                    {
                        Price = request.product.Price,
                        Name = request.product.Name,
                        Stock = request.product.Stock,
                    };

                    await _productWriteRepository.AddAsync(NewProduct);
                    await _productWriteRepository.SaveAsync();

                    // 201 Created dönüşü ve eklenen ürün bilgisi
                    return new CreateOneProductNoImageResponse() {
                        product = new()
                        {
                            Price = NewProduct.Price,
                            Name = NewProduct.Name,
                            Stock = NewProduct.Stock,
                            CreateTime = NewProduct.CreateTime,
                            UpdateTime= NewProduct.UpdateTime,
                            ID= NewProduct.ID,

                        },
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Ürün başarıyla Eklendi."
                    };
                }
            }
            catch (Exception ex)
            {
                // Genel bir hata mesajı dönüşü ve içerideki hata mesajı
                return new CreateOneProductNoImageResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = ex.ToString()
                };
            }
        }
    }
}

