using MediatR;

namespace OnionArch.Application.Features.Queries.BlogQueries.GetBlogBySlug
{
    public class GetBlogBySlugQueryRequest : IRequest<GetBlogBySlugQueryResponse>
    {
        public string blogSlug { get; set; }
    }
}