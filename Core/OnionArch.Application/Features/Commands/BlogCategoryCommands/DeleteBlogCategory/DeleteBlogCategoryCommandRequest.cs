using MediatR;

namespace OnionArch.Application.Features.Commands.BlogCategoryCommands.DeleteBlogCategory
{
    public class DeleteBlogCategoryCommandRequest : IRequest<DeleteBlogCategoryCommandResponse>
    {
        public string Id { get; set; }
    }
}
