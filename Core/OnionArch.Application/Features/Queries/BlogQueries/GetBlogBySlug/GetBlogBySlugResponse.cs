using OnionArch.Application.GlobalResponse;

namespace OnionArch.Application.Features.Queries.BlogQueries.GetBlogBySlug
{
    public class GetBlogBySlugQueryResponse : GlobalResponseResult
    {
        public object Blog { get; set; }
    }
}
