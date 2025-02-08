using MediatR;
using OnionArch.Application.Repositories.BlogCategoryCruds;
using OnionArch.Domain.Entities;
using System.Net;

namespace OnionArch.Application.Features.Commands.BlogCategoryCommands.CreateBlogCategory
{
    public class CreateBlogCategoryCommandHandler : IRequestHandler<CreateBlogCategoryCommandRequest, CreateBlogCategoryCommandResponse>
    {
        private readonly IBlogCategoryWriteRepository _blogCategoryWriteRepository;

        public CreateBlogCategoryCommandHandler(IBlogCategoryWriteRepository blogCategoryWriteRepository)
        {
            _blogCategoryWriteRepository = blogCategoryWriteRepository;
        }

        public async Task<CreateBlogCategoryCommandResponse> Handle(CreateBlogCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var blogCategory = new BlogCategory
                {
                    Name = request.Name,
                    Description = request.Description,
                    Slug = Common.CommonOperations.StringUtilities.GenerateSlug(request.Name)
                };

                await _blogCategoryWriteRepository.AddAsync(blogCategory);
                await _blogCategoryWriteRepository.SaveAsync();

                return new CreateBlogCategoryCommandResponse
                {
                    isCreated = true,
                    HassError = false,
                    Message = "Blog kategorisi başarıyla oluşturuldu",
                    StatusCode = HttpStatusCode.Created,
                    StatusCodeString = HttpStatusCode.Created.ToString()
                };
            }
            catch (Exception ex)
            {
                return new CreateBlogCategoryCommandResponse
                {
                    isCreated = false,
                    ErrorMessage = ex.Message,
                    HassError = true,
                    Message = "Blog kategorisi oluşturulurken bir hata oluştu",
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString()
                };
            }
        }
    }
}