using MediatR;
using OnionArch.Application.RequestParamaters;

namespace OnionArch.Application.Features.Queries.BlogQueries.GetAllBlogs
{
    public class GetAllBlogsQueryRequest : Pagination, IRequest<GetAllBlogsQueryResponse>
    {
        
        public bool? IsPublished { get; set; }
        public string? SearchByTitle { get; set; }
        public string? SearchByKeywords { get; set; }
        public string? CategorySlug { get; set; }
        public BlogSortBy? SortBy { get; set; }
    }

    public enum BlogSortBy
    {
        Newest = 0,      // En yeni
        Oldest = 1,      // En eski
        TitleAsc = 2,    // Baþlýk A'dan Z'ye
        TitleDesc = 3    // Baþlýk Z'den A'ya
    }
}