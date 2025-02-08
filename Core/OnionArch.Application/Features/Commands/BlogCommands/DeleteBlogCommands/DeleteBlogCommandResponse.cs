using OnionArch.Application.GlobalResponse;

namespace OnionArch.Application.Features.Commands.BlogCommands.DeleteBlogCommands
{
    public class DeleteBlogCommandResponse : GlobalResponseResult
    {
        public bool isDeleted { get; set; }
    }
}