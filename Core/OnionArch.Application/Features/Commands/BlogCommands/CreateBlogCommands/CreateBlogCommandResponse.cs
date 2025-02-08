using OnionArch.Application.GlobalResponse;

namespace OnionArch.Application.Features.Commands.BlogCommands.CreateBlogCommands
{
    public class CreateBlogCommandResponse : GlobalResponseResult
    {
        public bool isCreated { get; set; }
    }
}