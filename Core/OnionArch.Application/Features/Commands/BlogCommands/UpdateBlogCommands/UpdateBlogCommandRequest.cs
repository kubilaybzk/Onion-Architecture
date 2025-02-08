using MediatR;
using Microsoft.AspNetCore.Http;

namespace OnionArch.Application.Features.Commands.BlogCommands.UpdateBlogCommands
{
    public class UpdateBlogCommandRequest : IRequest<UpdateBlogCommandResponse>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishDate { get; set; }

        // SEO alanları
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }

        // Blog görseli
        public IFormFileCollection? CoverImage { get; set; }
    }
}