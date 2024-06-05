using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Features.Commands.Product.UpdateOneProduct;
using OnionArch.Domain.Entities;

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

            if (result)
            {
               
                    await _productWriteRepository.SaveAsync();
                return new DeleteProductByIdCommandsResponse
                {
                    isDeleted=true,
                    ErrorMessage = "",
                    HassError = false,
                    Message = "Silme başarıyla gerçekleşmiştir.",
                    StatusCode = System.Net.HttpStatusCode.Accepted,
                    StatusCodeString = System.Net.HttpStatusCode.Accepted.ToString(),
                };
            }
            else { 
                    return new DeleteProductByIdCommandsResponse
                    {
                        isDeleted=false,
                        ErrorMessage = "",
                        HassError = false,
                        Message = "Silme başarıyla başarısız.",
                        StatusCode = System.Net.HttpStatusCode.NotAcceptable,
                        StatusCodeString = System.Net.HttpStatusCode.NotAcceptable.ToString(),
                    };
                }

            }

        }
    }


