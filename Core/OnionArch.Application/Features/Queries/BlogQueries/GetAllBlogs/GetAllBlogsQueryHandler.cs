using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Repositories.BlogCruds;
using System.Net;

namespace OnionArch.Application.Features.Queries.BlogQueries.GetAllBlogs
{
    public class GetAllBlogsQueryHandler : IRequestHandler<GetAllBlogsQueryRequest, GetAllBlogsQueryResponse>
    {
        private readonly IBlogReadRepository _blogReadRepository;

        public GetAllBlogsQueryHandler(IBlogReadRepository blogReadRepository)
        {
            _blogReadRepository = blogReadRepository;
        }

        public async Task<GetAllBlogsQueryResponse> Handle(GetAllBlogsQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _blogReadRepository.Table
                    .Include(b => b.CoverImage)
                    .Include(b => b.Categories)
                    .AsQueryable();

                if (request.IsPublished.HasValue)
                    query = query.Where(b => b.IsPublished == request.IsPublished);

                if (!string.IsNullOrEmpty(request.SearchByTitle))
                    query = query.Where(b => b.Title.ToLower().Contains(request.SearchByTitle.ToLower()));

                if (!string.IsNullOrEmpty(request.CategorySlug))
                    query = query.Where(b => b.Categories.Any(c => c.Slug == request.CategorySlug));

                if (!string.IsNullOrEmpty(request.SearchByKeywords))
                    query = query.Where(b => b.SeoKeywords.ToLower().Contains(request.SearchByKeywords.ToLower()));

                var totalCount = await query.CountAsync();

                // Sıralama işlemleri
                query = request.SortBy switch
                {
                    BlogSortBy.Newest => query.OrderByDescending(b => b.CreateTime),
                    BlogSortBy.Oldest => query.OrderBy(b => b.CreateTime),
                    BlogSortBy.TitleAsc => query.OrderBy(b => b.Title),
                    BlogSortBy.TitleDesc => query.OrderByDescending(b => b.Title),
                    _ => query.OrderByDescending(b => b.PublishDate) // Default sıralama
                };


                var blogs = await query
                    .Skip(request.Page * request.Size)
                    .Take(request.Size)
                    .Select(b => new
                    {
                        b.ID,
                        b.Title,
                        b.Summary,
                        b.Slug,
                        b.IsPublished,
                        b.PublishDate,
                        b.ReadingTime,
                        b.SeoKeywords,
                        Categories = b.Categories.Select(c => new
                        {
                            c.ID,
                            c.Name,
                            c.Slug
                        }).ToList(),
                        CoverImage = b.CoverImage.FirstOrDefault(i => i.IsHeader) != null ? new
                        {
                            Path = b.CoverImage.First(i => i.IsHeader).Path,
                            FileName = b.CoverImage.First(i => i.IsHeader).FileName
                        } : null
                    })
                    .ToListAsync();

                return new GetAllBlogsQueryResponse
                {
                    Blogs = blogs,
                    TotalCount = totalCount,
                    CurrentPage = request.Page,
                    PageSize = request.Size,
                    HassError = false,
                    Message = "Blog yazıları başarıyla getirildi",
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString()
                };
            }
            catch (Exception ex)
            {
                return new GetAllBlogsQueryResponse
                {
                    Blogs = null,
                    ErrorMessage = ex.Message,
                    HassError = true,
                    Message = "Blog yazıları getirilirken bir hata oluştu",
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString()
                };
            }
        }
    }
}