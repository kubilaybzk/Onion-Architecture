using MediatR;
using OnionArch.Application.Features.Commands.Product.DeleteProductById;
using OnionArch.Application.Repositories.BrandCrud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.BrandCommands.DeleteBrandCommands
{
    public class DeleteBrandCommandsHandler : IRequestHandler<DeleteBrandCommandsRequest, DeleteBrandCommandsResponse>
    {
        
        private readonly IBrandWriteRepository _brandWriteRepository;

        public DeleteBrandCommandsHandler(IBrandWriteRepository brandWriteRepository)
        {
             
            _brandWriteRepository = brandWriteRepository;
        }

        public async Task<DeleteBrandCommandsResponse> Handle(DeleteBrandCommandsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var DeletedBrands = await _brandWriteRepository.RemoveAsync(request.DeletedBrandId);
                await _brandWriteRepository.SaveAsync();
                return new DeleteBrandCommandsResponse
                {
                    isDeleted = true,
                    ErrorMessage = "",
                    HassError = false,
                    Message = "Markayı Silme işlemi başarılı. ",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                };
            }
            catch (Exception ex)
            {
                return new DeleteBrandCommandsResponse
                {
                    isDeleted = false,
                    ErrorMessage = ex.Message,
                    HassError = true,
                    Message = "Markayı Silme işlemi başarısız. ",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString(),
                };
            }
        }
    }
}
