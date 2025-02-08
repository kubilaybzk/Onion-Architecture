using MediatR;
using OnionArch.Application.RequestParamaters;

namespace OnionArch.Application.Features.Queries.BlogCategoryQueries.GetAllBlogCategories
{
    public class GetAllBlogCategoriesQueryRequest : IRequest<GetAllBlogCategoriesQueryResponse>
    {
 
        public bool? IsActive { get; set; }
        public string? SearchByName { get; set; }
    }
}