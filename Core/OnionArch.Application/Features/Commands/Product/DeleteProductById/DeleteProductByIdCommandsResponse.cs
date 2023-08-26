using System;
namespace OnionArch.Application.Features.Commands.Product.DeleteProductById
{
	public class DeleteProductByIdCommandsResponse
	{
        public int StatusCode { get; set; } // HTTP durum kodunu içerecek bir özellik ekledik
        public string Message { get; set; } // Opsiyonel: Bir hata mesajı ekleyebilirsiniz
    }
}

