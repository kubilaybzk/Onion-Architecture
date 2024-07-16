using MediatR;
using OnionArch.Application.Repositories.AttributeCrud.CategoryAttributeCrud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.ProductFilterComands.DeleteCategoryAttributeFilter
{
    public class DeleteCategoryAttributeFilterHandler : IRequestHandler<DeleteCategoryAttributeFilterRequest, DeleteCategoryAttributeFilterResponse>
    {
        private readonly ICategoryAttributeReadRepository _categoryAttributeReadRepository;
        private readonly ICategoryAttributeWriteRepository _categoryAttributeWriteRepository;

        public DeleteCategoryAttributeFilterHandler(ICategoryAttributeReadRepository categoryAttributeReadRepository, ICategoryAttributeWriteRepository categoryAttributeWriteRepository)
        {
            _categoryAttributeReadRepository = categoryAttributeReadRepository;
            _categoryAttributeWriteRepository = categoryAttributeWriteRepository;
        }


        public async Task<DeleteCategoryAttributeFilterResponse> Handle(DeleteCategoryAttributeFilterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var targetCategoryFilter = await _categoryAttributeReadRepository.GetByIdAsync(request.CategoryFilterId);
                if (targetCategoryFilter != null)
                {
                    await _categoryAttributeWriteRepository.RemoveAsync(request.CategoryFilterId);
                    await _categoryAttributeWriteRepository.SaveAsync();
                    return new DeleteCategoryAttributeFilterResponse()
                    {
                        isDeleted = true,
                        Message = "Silme işlemi başarılı",
                        HassError = false,
                        StatusCode = System.Net.HttpStatusCode.OK,
                        StatusCodeString = System.Net.HttpStatusCode.OK.ToString()
                    };
                }
                else
                {
                    return new DeleteCategoryAttributeFilterResponse()
                    {
                        isDeleted = false,
                        Message = "Filtre bulunamadı.",
                        HassError = true,
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        StatusCodeString = System.Net.HttpStatusCode.NotFound.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                return new DeleteCategoryAttributeFilterResponse()
                {
                    isDeleted = false,
                    ErrorMessage = ex.Message,
                    Message = "Silme işlemi başarısız",
                    HassError=true,
                    StatusCode=System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString= System.Net.HttpStatusCode.InternalServerError.ToString()


                };
            }
        }
    }
}
