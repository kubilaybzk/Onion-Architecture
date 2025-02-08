using OnionArch.Application.GlobalResponse;

namespace OnionArch.Application.Features.Commands.BlogCategoryCommands.UpdateBlogCategory
{
    public class UpdateBlogCategoryCommandResponse : GlobalResponseResult
    {
        public bool isUpdated { get; set; }
    }
}
