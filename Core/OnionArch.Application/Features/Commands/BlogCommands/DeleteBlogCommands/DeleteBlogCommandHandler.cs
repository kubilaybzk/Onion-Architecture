using MediatR;
using OnionArch.Application.Repositories.BlogCruds;
using System.Net;

namespace OnionArch.Application.Features.Commands.BlogCommands.DeleteBlogCommands
{
    public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommandRequest, DeleteBlogCommandResponse>
    {
        private readonly IBlogWriteRepository _blogWriteRepository;
        private readonly IBlogReadRepository _blogReadRepository;

        public DeleteBlogCommandHandler(IBlogWriteRepository blogWriteRepository, IBlogReadRepository blogReadRepository)
        {
            _blogWriteRepository = blogWriteRepository;
            _blogReadRepository = blogReadRepository;
        }

        public async Task<DeleteBlogCommandResponse> Handle(DeleteBlogCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var blog = await _blogReadRepository.GetByIdAsync(request.Id);
                if (blog == null)
                    throw new Exception("Blog bulunamadı");

                await _blogWriteRepository.RemoveAsync(request.Id);
                await _blogWriteRepository.SaveAsync();

                return new DeleteBlogCommandResponse
                {
                    isDeleted = true,
                    HassError = false,
                    Message = "Blog yazısı başarıyla silindi",
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString()
                };
            }
            catch (Exception ex)
            {
                return new DeleteBlogCommandResponse
                {
                    isDeleted = false,
                    ErrorMessage = ex.Message,
                    HassError = true,
                    Message = "Blog yazısı silinirken bir hata oluştu",
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString()
                };
            }
        }
    }
}