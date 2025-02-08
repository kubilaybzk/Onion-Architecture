using MediatR;

namespace OnionArch.Application.Features.Queries.BlogQueries.GetBlogById
{
    public class GetBlogByIdQueryRequest : IRequest<GetBlogByIdQueryResponse>
    {
        public string Id { get; set; }
    }
}