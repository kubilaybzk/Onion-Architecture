using MediatR;
using OnionArch.Application.CQRS_Globals.Pagination;
using OnionArch.Application.Features.Queries.Product.GetProductByCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.BrandQueries.GetAllProductByBrandNameQueries
{
    public class GetAllProductByBrandNameRequest:IRequest<GetAllProductByBrandNameResponse>
    {
        public string BrandName { get; set; }
        public GlobalPaginationRequest PaginationValues { get; set; }
        public Dictionary<string, List<string>>? AttributeFilters { get; set; }
        public ProductSortOption SortOption { get; set; } = ProductSortOption.DateNew; // Default değer
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
    
}
