using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Features.Commands.CategoryCommands.DeleteCategoryComands;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.CategoryCommands.UpdateCategoryComands
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryRequest, UpdateCategoryResponse>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly ICategoryWriteRepository _categoryWriteRepository;
        public UpdateCategoryHandler(ICategoryReadRepository categoryReadRepository, ICategoryWriteRepository categoryWriteRepository)
        {
            _categoryReadRepository = categoryReadRepository;
            _categoryWriteRepository = categoryWriteRepository;
        }

        public async Task<UpdateCategoryResponse> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {

            try
            {
                Category targetCategory = await _categoryReadRepository.GetByIdAsync(request.ID ,true);

                targetCategory.CategorySlug = request.CategorySlug;
                targetCategory.CategoryName = request.CategoryName;
                targetCategory.CategoryLinkTitle = request.CategoryLinkTitle;
                targetCategory.ParentCategoryId = request.ParentCategoryId;
                targetCategory.CategoryHasTitleImage = request.CategoryHasTitleImage;
                targetCategory.CategorySlug = request.CategorySlug;
                targetCategory.CategoryDisplayStatus = request.CategoryDisplayStatus;
                targetCategory.CategoryOrder = request.CategoryOrder;
                await _categoryWriteRepository.SaveAsync();
                return new UpdateCategoryResponse()
                {
                    ErrorMessage = null,
                    HassError = false,
                    Message = "Update işlemi başarılı",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = System.Net.HttpStatusCode.OK.ToString()

                };
            }
            catch (Exception ex)
            {
                return new UpdateCategoryResponse()
                {
                    ErrorMessage = ex.Message,
                    HassError = true,
                    Message = "Update işlemi yapılırken bir hata alındı",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString()

                };
            }
           
        }
    }
}
