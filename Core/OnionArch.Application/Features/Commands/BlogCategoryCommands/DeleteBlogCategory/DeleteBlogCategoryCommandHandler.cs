using MediatR;
using OnionArch.Application.Repositories.BlogCategoryCruds;
using System.Net;

namespace OnionArch.Application.Features.Commands.BlogCategoryCommands.DeleteBlogCategory
{
    public class DeleteBlogCategoryCommandHandler : IRequestHandler<DeleteBlogCategoryCommandRequest, DeleteBlogCategoryCommandResponse>
    {
        private readonly IBlogCategoryWriteRepository _blogCategoryWriteRepository;
        private readonly IBlogCategoryReadRepository _blogCategoryReadRepository;

        public DeleteBlogCategoryCommandHandler(IBlogCategoryWriteRepository blogCategoryWriteRepository, IBlogCategoryReadRepository blogCategoryReadRepository)
        {
            _blogCategoryWriteRepository = blogCategoryWriteRepository;
            _blogCategoryReadRepository = blogCategoryReadRepository;
        }

        public async Task<DeleteBlogCategoryCommandResponse> Handle(DeleteBlogCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var blogCategory = await _blogCategoryReadRepository.GetByIdAsync(request.Id);
                if (blogCategory == null)
                    throw new Exception("Blog kategorisi bulunamadı");

                await _blogCategoryWriteRepository.RemoveAsync(request.Id);
                await _blogCategoryWriteRepository.SaveAsync();

                return new DeleteBlogCategoryCommandResponse
                {
                    isDeleted = true,
                    HassError = false,
                    Message = "Blog kategorisi başarıyla silindi",
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString()
                };
            }
            catch (Exception ex)
            {
                return new DeleteBlogCategoryCommandResponse
                {
                    isDeleted = false,
                    ErrorMessage = ex.Message,
                    HassError = true,
                    Message = "Blog kategorisi silinirken bir hata oluştu",
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString()
                };
            }
        }
    }
}