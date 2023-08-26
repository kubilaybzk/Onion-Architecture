using System;
using p = OnionArch.Domain.Entities;
namespace OnionArch.Application.Features.Commands.Product.UpdateOneProduct
{
    public class UpdateOneProductResponse
    {
        public object Product { get; set; }
        public int StatusCode { get; set; } // HTTP durum kodunu içerecek bir özellik ekledik
        public string Message { get; set; } // Opsiyonel: Bir hata mesajı ekleyebilirsiniz
    }
}

