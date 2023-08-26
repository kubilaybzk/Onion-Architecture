using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Features.Commands.Product.UpdateOneProduct;

namespace OnionArch.Application.Features.Commands.Product.DeleteProductById
{
    public class DeleteProductByIdCommandsHandler : IRequestHandler<DeleteProductByIdCommandsRequest, DeleteProductByIdCommandsResponse>
    {

        private readonly IProductWriteRepository _productWriteRepository;

        public DeleteProductByIdCommandsHandler(IProductWriteRepository productWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
        }

        public async Task<DeleteProductByIdCommandsResponse> Handle(DeleteProductByIdCommandsRequest request, CancellationToken cancellationToken)
        {
            var result = await _productWriteRepository.RemoveAsync(request.id);

            switch (result)
            {
                case true:
                    await _productWriteRepository.SaveAsync();
                    return new DeleteProductByIdCommandsResponse
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Ürün başarıyla silindi."
                    };
                case false:
                    return new DeleteProductByIdCommandsResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Ürün bulunamadı."
                    };
                default:
                    return new DeleteProductByIdCommandsResponse
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = "Sunucu kaynaklı bir hata"
                    };
                    
            }

        }
    }
}

