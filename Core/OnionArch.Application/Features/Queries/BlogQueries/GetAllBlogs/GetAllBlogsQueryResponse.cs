using OnionArch.Application.GlobalResponse;

namespace OnionArch.Application.Features.Queries.BlogQueries.GetAllBlogs
{
    public class GetAllBlogsQueryResponse : GlobalResponseResult
    {
        public object Blogs { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public bool HasNext => TotalCount > (CurrentPage + 1) * PageSize;
        public bool HasPrev => CurrentPage > 0;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}