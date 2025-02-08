using OnionArch.Application.GlobalResponse;

namespace OnionArch.Application.Features.Commands.BlogCategoryCommands.DeleteBlogCategory
{
    public class DeleteBlogCategoryCommandResponse : GlobalResponseResult
    {
        public bool isDeleted { get; set; }
    }
}
