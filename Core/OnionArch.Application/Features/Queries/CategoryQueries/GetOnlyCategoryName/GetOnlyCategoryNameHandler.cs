using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Features.Queries.CategoryQueries.GetAllCategory;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Application.View_Models.Category;
using OnionArch.Domain.Entities;
using System.Net;

namespace OnionArch.Application.Features.Queries.CategoryQueries.GetOnlyCategoryName
{
    public class GetOnlyCategoryNameHandler : IRequestHandler<GetOnlyCategoryNameRequest, GetOnlyCategoryNameResponse>
    {

        private readonly ICategoryReadRepository _categoryReadRepository;

        public GetOnlyCategoryNameHandler(ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;
        }

        public async Task<GetOnlyCategoryNameResponse> Handle(GetOnlyCategoryNameRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _categoryReadRepository.GetAll()
                    .Include(p => p.SubCategories)
                    .ToListAsync(cancellationToken);

                var topLevelCategories = data
                    .Where(c => c.ParentCategoryId == null)
                    .OrderBy(p => p.CategoryOrder)
                    .ToList();

                var categoryViewModels = topLevelCategories
                    .Select(c => MapToViewModel(c, data))
                    .ToList();

                var response = new GetOnlyCategoryNameResponse
                {
                    Categories = categoryViewModels,
                    Message = "Başarıyla Kategoriler gönderildi",
                    HassError = false,
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString(),
                    ErrorMessage = ""
                };

                return response;
            }
            catch (Exception ex)
            {
                var response = new GetOnlyCategoryNameResponse
                {
                    Categories = null,
                    Message = "Kategoriler oluşturulurken bir hata ile karşılaşıldı.",
                    HassError = true,
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                    ErrorMessage = "Alınan hatanın kodu: " + ex.Message
                };
                return response;
            }
        }

        private VM_GetOnlyCategoryName_VM MapToViewModel(Category category, List<Category> allCategories)
        {
            return new VM_GetOnlyCategoryName_VM
            {
                Id = category.ID,
                CategoryName = category.CategoryName,
                IsSpecialCategory=category.IsSpecialCategory,
                SubCategories = allCategories
                    .Where(c => c.ParentCategoryId == category.ID)
                    .Select(subCategory => MapToViewModel(subCategory, allCategories))
                    .OrderBy(sc => sc.CategoryName) // Optional: Order by CategoryName or any other property
                    .ToList()
            };
        }
    }
}
