using MediatR;
using OnionArch.Application.Features.Queries.Product.Product.GetAllProducts;

namespace OnionArch.Application.Features.Queries.Product.GetAllProducts
{
    //MediatR  tarafından oluşturulan interfaceleri tanımlamamız gerekiyo.  
    public class GetAllProductsQueryRequest:IRequest<GetAllProductsQueryResponse>
	{
        //Mevcut olan yapının içinde bulunan parametrelerimizi burada belirtiyoruz.

        //public Pagination Pagination { get; set; } = new Pagination
        //{

        //    Page = 0,
        //    Size = 8
        //};

        public int Page { get; set; } = 0;
        public int Size { get; set; } = 8;

    }
}

