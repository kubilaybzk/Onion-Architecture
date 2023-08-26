using System;
namespace OnionArch.Application.Features.Queries.Product.Product.GetAllProducts
{
	public class GetAllProductsQueryResponse
	{
		public int TotalCount { get; set; }
        public int TotalPageSize { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrev { get; set; }
        public int PageSize { get; set; }
        public object Products { get; set;}
        public int StatusCode { get; set; } // HTTP durum kodunu içerecek bir özellik ekledik
        public string Message { get; set; } // Opsiyonel: Bir hata mesajı ekleyebilirsiniz

    }
}

