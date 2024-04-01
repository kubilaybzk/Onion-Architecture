using Google.Apis.Logging;
using MediatR;
using OnionArch.Application.Abstractions.FileCrud;
using OnionArch.Application.Abstractions.ProductImageFileCrud;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.CategoryQueries.DeleteCategory
{
    internal class DeleteCategoryHandler : IRequestHandler<DeleteCategoryRequest, DeleteCategoryResponse>
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
                    HassError = false,
                    Message="Kategori silme işlemi başarılı",
                    StatusCode=System.Net.HttpStatusCode.Created,
                };

            }
            catch (Exception ex)
            {
               
                return new DeleteCategoryResponse()
                {
                    HassError = true,
                    Message = "Kategori silme işlemi başarısız Hata Kodu " + ex.Message.ToString(),
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                };
            }

        }
    }
}
