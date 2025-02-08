using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Repositories.BlogCruds;
using OnionArch.Domain.Entities;
using System.Net;

namespace OnionArch.Application.Features.Commands.BlogCommands.UpdateBlogCommands
{
    public class UpdateBlogCommandHandler : IRequestHandler<UpdateBlogCommandRequest, UpdateBlogCommandResponse>
    {
        private readonly IBlogWriteRepository _blogWriteRepository;
        private readonly IBlogReadRepository _blogReadRepository;
        private readonly IStorageService _storageService;

        public UpdateBlogCommandHandler(IBlogWriteRepository blogWriteRepository, IBlogReadRepository blogReadRepository, IStorageService storageService)
        {
            _blogWriteRepository = blogWriteRepository;
            _blogReadRepository = blogReadRepository;
            _storageService = storageService;
        }

        public async Task<UpdateBlogCommandResponse> Handle(UpdateBlogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var blog = await _blogReadRepository.Table
                    .Include(b => b.CoverImage)
                    .FirstOrDefaultAsync(b => b.ID == Guid.Parse(request.Id));

                if (blog == null)
                    throw new Exception("Blog bulunamadı");

                blog.Title = request.Title;
                blog.Content = request.Content;
                blog.Summary = request.Summary;
                blog.IsPublished = request.IsPublished;
                blog.PublishDate = request.PublishDate;
                blog.SeoTitle = request.SeoTitle;
                blog.SeoDescription = request.SeoDescription;
                blog.SeoKeywords = request.SeoKeywords;
                blog.Slug = GenerateSlug(request.Title);
                blog.ReadingTime = CalculateReadingTime(request.Content);

                if (request.CoverImage != null && request.CoverImage.Count > 0)
                {
                    await _storageService.DeleteFileAsync(blog.CoverImage.FirstOrDefault().FileName, "wwwroot/resource/blog-images");
                    var result = await _storageService.UploadAsync("blog-images", request.CoverImage);
                    blog.CoverImage = new List<BlogImageFile>
                    {
                        new BlogImageFile
                        {
                            FileName = result.First().fileName,
                            Path = result.First().PathOrContainerName,
                            Storage = _storageService.StorageType,
                            IsHeader = true
                        }
                    };
                }

                _blogWriteRepository.Update(blog);
                await _blogWriteRepository.SaveAsync();

                return new UpdateBlogCommandResponse
                {
                    isUpdated = true,
                    HassError = false,
                    Message = "Blog yazısı başarıyla güncellendi",
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString()
                };
            }
            catch (Exception ex)
            {
                return new UpdateBlogCommandResponse
                {
                    isUpdated = false,
                    ErrorMessage = ex.Message,
                    HassError = true,
                    Message = "Blog yazısı güncellenirken bir hata oluştu",
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString()
                };
            }
        }

        private string GenerateSlug(string title)
        {
            return Common.CommonOperations.StringUtilities.GenerateSlug(title);
        }

        private int CalculateReadingTime(string content)
        {
            const int wordsPerMinute = 200;
            var wordCount = content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
            return (int)Math.Ceiling((double)wordCount / wordsPerMinute);
        }
    }
}