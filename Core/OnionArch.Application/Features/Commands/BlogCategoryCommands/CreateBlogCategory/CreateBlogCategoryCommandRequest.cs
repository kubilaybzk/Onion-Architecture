using MediatR;

namespace OnionArch.Application.Features.Commands.BlogCategoryCommands.CreateBlogCategory
{
    public class CreateBlogCategoryCommandRequest : IRequest<CreateBlogCategoryCommandResponse>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
    }
}
