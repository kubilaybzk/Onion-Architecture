using OnionArch.Application.GlobalResponse;
using OnionArch.Application.View_Models.Product;
using System;
namespace OnionArch.Application.Features.Queries.Product.Product.GetAllProducts
{
	public class GetAllProductsQueryResponse:GlobalResponseResult
	{
		public int TotalCount { get; set; }
        public int TotalPageSize { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrev { get; set; }
        public int PageSize { get; set; }
        public object Products { get; set;}

    }
}

