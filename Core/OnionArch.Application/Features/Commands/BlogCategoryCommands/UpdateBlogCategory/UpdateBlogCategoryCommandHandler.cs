using MediatR;
using OnionArch.Application.Repositories.BlogCategoryCruds;
using System.Net;

namespace OnionArch.Application.Features.Commands.BlogCategoryCommands.UpdateBlogCategory
{
    public class UpdateBlogCategoryCommandHandler : IRequestHandler<UpdateBlogCategoryCommandRequest, UpdateBlogCategoryCommandResponse>
    {
        private readonly IBlogCategoryWriteRepository _blogCategoryWriteRepository;
        private readonly IBlogCategoryReadRepository _blogCategoryReadRepository;

        public UpdateBlogCategoryCommandHandler(IBlogCategoryWriteRepository blogCategoryWriteRepository, IBlogCategoryReadRepository blogCategoryReadRepository)
        {
            _blogCategoryWriteRepository = blogCategoryWriteRepository;
            _blogCategoryReadRepository = blogCategoryReadRepository;
        }

        public async Task<UpdateBlogCategoryCommandResponse> Handle(UpdateBlogCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var blogCategory = await _blogCategoryReadRepository.GetByIdAsync(request.Id);
                if (blogCategory == null)
                    throw new Exception("Blog kategorisi bulunamadı");

                blogCategory.Name = request.Name;
                blogCategory.Description = request.Description;
                blogCategory.IsActive = request.IsActive;
                blogCategory.DisplayOrder = request.DisplayOrder;
                blogCategory.Slug = Common.CommonOperations.StringUtilities.GenerateSlug(request.Name);

                _blogCategoryWriteRepository.Update(blogCategory);
                await _blogCategoryWriteRepository.SaveAsync();

                return new UpdateBlogCategoryCommandResponse
                {
                    isUpdated = true,
                    HassError = false,
                    Message = "Blog kategorisi başarıyla güncellendi",
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString()
                };
            }
            catch (Exception ex)
            {
                return new UpdateBlogCategoryCommandResponse
                {
                    isUpdated = false,
                    ErrorMessage = ex.Message,
                    HassError = true,
                    Message = "Blog kategorisi güncellenirken bir hata oluştu",
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString()
                };
            }
        }
    }
}
