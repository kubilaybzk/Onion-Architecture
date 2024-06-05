using OnionArch.Application.GlobalResponse;
using System;
namespace OnionArch.Application.Features.Commands.Product.DeleteProductById
{
	public class DeleteProductByIdCommandsResponse:GlobalResponseResult
	{
        public bool isDeleted { get; set; }
    }
}

