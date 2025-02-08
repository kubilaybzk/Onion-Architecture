using OnionArch.Application.GlobalResponse;

namespace OnionArch.Application.Features.Commands.BlogCategoryCommands.CreateBlogCategory
{
    public class CreateBlogCategoryCommandResponse : GlobalResponseResult
    {
        public bool isCreated { get; set; }
    }
}
