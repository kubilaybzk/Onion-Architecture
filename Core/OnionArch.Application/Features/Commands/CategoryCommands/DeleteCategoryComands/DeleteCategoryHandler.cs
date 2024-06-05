using MediatR;
using OnionArch.Application.Repositories.CategoryCrud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.CategoryCommands.DeleteCategoryComands
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryRequest, DeleteCategoryResponse>
    {

        public readonly ICategoryWriteRepository _categoryWriteRepository;


        public DeleteCategoryHandler(ICategoryWriteRepository categoryWriteRepository)
        {
            _categoryWriteRepository = categoryWriteRepository;
        }

        public async Task<DeleteCategoryResponse> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {

                await _categoryWriteRepository.RemoveAsync(request.CategoryId);
                await _categoryWriteRepository.SaveAsync();
                return new DeleteCategoryResponse()
                {
                    isDeleted = true,
                    HassError = false,
                    Message = "Kategori silme işlemi başarılı",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = System.Net.HttpStatusCode.OK.ToString()
                };
       
            }
            catch (Exception ex)
            {

                return new DeleteCategoryResponse()
                {
                    isDeleted=false,
                    HassError = true,
                    Message = "Kategori silme işlemi başarısız ",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString()
                };
            };
        }

    }
}
 
