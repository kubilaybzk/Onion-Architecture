using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnionArch.Application.Features.Queries.Product.GetAllProducts;
using OnionArch.Application.Repositories.BackEndLogsCrud;

namespace OnionArch.Application.Features.Queries.BackEndLogs
{
    public class BackEndLogsQueryHandler : IRequestHandler<BackEndLogsQueryRequest, BackEndLogsQueryResponse>
    {
       
        private readonly IBackEndLogsReadRepository _backEndLogsReadRepository;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        public BackEndLogsQueryHandler(IBackEndLogsReadRepository backEndLogsReadRepository, ILogger<GetAllProductsQueryHandler> logger)
        {
            _backEndLogsReadRepository = backEndLogsReadRepository;
            _logger = logger;
        }

        public async Task<BackEndLogsQueryResponse> Handle(BackEndLogsQueryRequest request, CancellationToken cancellationToken)
        {


            try
            {
                var BackendLogsQuery = _backEndLogsReadRepository.GetAll();
                int totalProductCount = await BackendLogsQuery.CountAsync();


                int totalPageSize = (int)Math.Ceiling((double)totalProductCount / request.Size);
                var pagedBackendLogsQuery = BackendLogsQuery.Skip(request.Size * request.Page).Take(request.Size);
                int pageSize = await pagedBackendLogsQuery.CountAsync();
                var BackendLogsResult = await pagedBackendLogsQuery
                    .Select(p => new
                    {
                        p.Message,
                        p.EmailOrUserNameLogs,
                        p.MessageTemplate,
                        p.CreateTime,
                        p.UpdateTime,
                        p.Exception
                    })
                    .ToListAsync();

                bool hasNextPage = request.Page < totalPageSize - 1;
                bool hasPrevPage = request.Page > 0;
                _logger.LogInformation("Başarılı bir şekilde BackendLog listelendi");

                return new BackEndLogsQueryResponse()
                {
                    TotalCount = totalProductCount,
                    TotalPageSize = totalPageSize,
                    CurrentPage = request.Page,
                    HasNext = hasNextPage,
                    HasPrev = hasPrevPage,
                    PageSize = pageSize,
                    BackEndLogs = BackendLogsResult,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Ürün Gönderildi"
                   
                };
            }
            catch (Exception ex) {
                _logger.LogError("BackendLog listelerken bir hata oluştu.");
                return new BackEndLogsQueryResponse()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = ex.Message.ToString()
                };
            }

            throw new NotImplementedException();
        }
    }
}
