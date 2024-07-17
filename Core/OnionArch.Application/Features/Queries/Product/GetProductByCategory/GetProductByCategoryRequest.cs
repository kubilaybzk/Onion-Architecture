using MediatR;
using OnionArch.Application.CQRS_Globals.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Product.GetProductByCategory
{
    public class GetProductByCategoryRequest :IRequest<GetProductByCategoryResponse>
    {
        public string? CategorySlug { get; set; }
        public string? Id { get; set; }
        public string? CategoryName { get; set; }
        public GlobalPaginationRequest  PaginationValues { get; set; }

        public Dictionary<string, List<string>>? AttributeFilters { get; set; }
        public ProductSortOption SortOption { get; set; } = ProductSortOption.DateNew; // Default değer
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }

    public enum ProductSortOption
    {
        DateNew, // Eklenme tarihine göre yeniden eskiye (default)
        DateOld, // Eklenme tarihine göre eskiden yeniye
        NameAsc, // İsme göre A'dan Z'ye
        NameDesc, // İsme göre Z'den A'ya
        PriceAsc, // Fiyata göre düşükten yükseğe
        PriceDesc // Fiyata göre yüksekten düşüğe
    }
}
