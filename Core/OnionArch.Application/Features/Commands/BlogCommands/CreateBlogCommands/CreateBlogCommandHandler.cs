using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Repositories.BlogCruds;
using OnionArch.Application.Repositories.BlogCategoryCruds;
using OnionArch.Domain.Entities;
using System.Net;

namespace OnionArch.Application.Features.Commands.BlogCommands.CreateBlogCommands
{
    public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommandRequest, CreateBlogCommandResponse>
    {
        private readonly IBlogWriteRepository _blogWriteRepository;
        private readonly IStorageService _storageService;
        private readonly IBlogCategoryReadRepository _blogCategoryReadRepository;

        public CreateBlogCommandHandler(IBlogWriteRepository blogWriteRepository, IStorageService storageService, IBlogCategoryReadRepository blogCategoryReadRepository)
        {
            _blogWriteRepository = blogWriteRepository;
            _storageService = storageService;
            _blogCategoryReadRepository = blogCategoryReadRepository;
        }

        public async Task<CreateBlogCommandResponse> Handle(CreateBlogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Seçilen kategorileri getir
                var categories = await _blogCategoryReadRepository.Table
                    .Where(c => request.CategoryIds.Contains(c.ID))
                    .ToListAsync();

                if (!categories.Any())
                    throw new Exception("En az bir kategori seçilmelidir");

                var blog = new Blog
                {
                    Title = request.Title,
                    Content = request.Content,
                    Summary = request.Summary,
                    IsPublished = request.IsPublished,
                    PublishDate = DateTime.UtcNow,
                    SeoTitle = request.SeoTitle,
                    SeoDescription = request.SeoDescription,
                    SeoKeywords = request.SeoKeywords,
                    Slug = GenerateSlug(request.Title),
                    ReadingTime = CalculateReadingTime(request.Content),
                    Categories = categories
                };

                if (request.CoverImage != null && request.CoverImage.Count > 0)
                {
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

                await _blogWriteRepository.AddAsync(blog);
                await _blogWriteRepository.SaveAsync();

                return new CreateBlogCommandResponse
                {
                    isCreated = true,
                    HassError = false,
                    Message = "Blog yazısı başarıyla oluşturuldu",
                    StatusCode = HttpStatusCode.Created,
                    StatusCodeString = HttpStatusCode.Created.ToString()
                };
            }
            catch (Exception ex)
            {
                return new CreateBlogCommandResponse
                {
                    isCreated = false,
                    ErrorMessage = ex.Message,
                    HassError = true,
                    Message = "Blog yazısı oluşturulurken bir hata oluştu",
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