using System;
using OnionArch.Domain.Entities;
namespace OnionArch.Application.Features.Queries.Product.GetSingleById
{
	public class GetSingleByIdQueryResponse
	{
        public object Products { get; set; }
        public int StatusCode { get; set; } // HTTP durum kodunu içerecek bir özellik ekledik
        public string Message { get; set; } // Opsiyonel: Bir hata mesajı ekleyebilirsiniz
    }
}

