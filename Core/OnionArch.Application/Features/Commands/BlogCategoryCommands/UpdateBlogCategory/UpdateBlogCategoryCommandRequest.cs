using MediatR;

namespace OnionArch.Application.Features.Commands.BlogCategoryCommands.UpdateBlogCategory
{
    public class UpdateBlogCategoryCommandRequest : IRequest<UpdateBlogCategoryCommandResponse>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }
}
