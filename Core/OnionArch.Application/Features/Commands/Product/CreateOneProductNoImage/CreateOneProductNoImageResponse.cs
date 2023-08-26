using System;
using p = OnionArch.Domain.Entities;
namespace OnionArch.Application.Features.Commands.Product.CreateOneProductNoImage
{
	public class CreateOneProductNoImageResponse
	{
		public p.Product product { get; set; }
        public int StatusCode { get; set; } // HTTP durum kodunu içerecek bir özellik ekledik
        public string Message { get; set; } // Opsiyonel: Bir hata mesajı ekleyebilirsiniz
    }
}

