using MediatR;

namespace OnionArch.Application.Features.Commands.BlogCommands.DeleteBlogCommands
{
    public class DeleteBlogCommandRequest : IRequest<DeleteBlogCommandResponse>
    {
        public string Id { get; set; }
    }
}