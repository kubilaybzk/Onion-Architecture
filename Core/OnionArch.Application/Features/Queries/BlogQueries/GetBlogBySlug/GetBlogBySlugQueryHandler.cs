using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Repositories.BlogCruds;
using OnionArch.Domain.Entities;
using System.Net;

namespace OnionArch.Application.Features.Queries.BlogQueries.GetBlogBySlug
{
    public class GetBlogBySlugQueryHandler : IRequestHandler<GetBlogBySlugQueryRequest, GetBlogBySlugQueryResponse>
    {
        private readonly IBlogReadRepository _blogReadRepository;

        public GetBlogBySlugQueryHandler(IBlogReadRepository blogReadRepository)
        {
            _blogReadRepository = blogReadRepository;
        }

        public async Task<GetBlogBySlugQueryResponse> Handle(GetBlogBySlugQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var blog = await _blogReadRepository.Table
                    .Include(b => b.CoverImage)
                    .Select(b => new
                    {
                        b.ID,
                        b.Title,
                        b.Content,
                        b.Summary,
                        b.Slug,
                        b.IsPublished,
                        b.PublishDate,
                        b.ReadingTime,
                        b.SeoTitle,
                        b.SeoDescription,
                        b.SeoKeywords,
                        Categories=b.Categories.Select(p=> new BlogCategory()
                        {
                            ID=p.ID,
                            Name=p.Name,
                            Slug=p.Slug,
                            Description=p.Description
                        }),
                        CoverImage = b.CoverImage.FirstOrDefault(i => i.IsHeader) != null ? new
                        {
                            Path = b.CoverImage.First(i => i.IsHeader).Path,
                            FileName = b.CoverImage.First(i => i.IsHeader).FileName
                        } : null
                    })
                    .FirstOrDefaultAsync(b => b.Slug == request.blogSlug);

                if (blog == null)
                    throw new Exception("Blog bulunamadı");

                return new GetBlogBySlugQueryResponse
                {
                    Blog = blog,
                    HassError = false,
                    Message = "Blog yazısı başarıyla getirildi",
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString()
                };
            }
            catch (Exception ex)
            {
                return new GetBlogBySlugQueryResponse
                {
                    Blog = null,
                    ErrorMessage = ex.Message,
                    HassError = true,
                    Message = "Blog yazısı getirilirken bir hata oluştu",
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString()
                };
            }
        }
    }
}