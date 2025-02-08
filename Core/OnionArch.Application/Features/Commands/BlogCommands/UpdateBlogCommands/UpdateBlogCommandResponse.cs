using OnionArch.Application.GlobalResponse;

namespace OnionArch.Application.Features.Commands.BlogCommands.UpdateBlogCommands
{
    public class UpdateBlogCommandResponse : GlobalResponseResult
    {
        public bool isUpdated { get; set; }
    }
}
