using OnionArch.Application.GlobalResponse;

namespace OnionArch.Application.Features.Queries.BlogQueries.GetBlogById
{
    public class GetBlogByIdQueryResponse : GlobalResponseResult
    {
        public object Blog { get; set; }
    }
}
