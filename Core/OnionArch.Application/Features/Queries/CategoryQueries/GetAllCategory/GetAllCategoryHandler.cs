using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Domain.Entities;
using System.Net;

namespace OnionArch.Application.Features.Queries.CategoryQueries.GetAllCategory
{
    public class GetAllCategoryHandler : IRequestHandler<GetAllCategoryRequest, GetAllCategoryResponse>
    {
        public readonly ICategoryReadRepository _categoryReadRepository;

        public GetAllCategoryHandler(ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;
        }



        public async Task<GetAllCategoryResponse> Handle(GetAllCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var data2 = _categoryReadRepository.GetAll().Include(p => p.CategoryImageFiles) ; // Verilerin asenkron olarak alınması

                var data = data2.Select(p => new Category()
                {
                    CategoryName = p.CategoryName,
                    CategoryImageFiles = p.CategoryImageFiles.Select(bas => new CategoryImageFile()
                    {
                        FileName = bas.FileName,
                        Path = bas.Path,
                        Storage = bas.Storage,
                        ImageTitle = bas.ImageTitle,
                        IsHeaderImage = bas.IsHeaderImage,
                        ID = bas.ID,
                        ShowImage = bas.ShowImage,
                        CreateTime = bas.CreateTime,
                        UpdateTime = bas.UpdateTime,
                        CategoryImageOrder = bas.CategoryImageOrder,
                        CategoryRedirectLink = bas.CategoryRedirectLink,
                        CategoryRedirectLinkTitle = bas.CategoryRedirectLinkTitle,
                        
                    }).OrderBy(p=>p.CategoryImageOrder).ToList(),
                    CategorySlug = p.CategorySlug,
                    ParentCategoryId = p.ParentCategoryId,
                    SubCategories = p.SubCategories,
                    CategoryDisplayStatus = p.CategoryDisplayStatus,
                    CategoryHasTitleImage = p.CategoryHasTitleImage,
                    CategoryLinkTitle = p.CategoryLinkTitle,
                    CategoryOrder = p.CategoryOrder,
                    ID = p.ID,
                    IsSpecialCategory = p.IsSpecialCategory,
                    IsCampanyCategory = p.IsCampanyCategory,
                    MaterializedPath = p.MaterializedPath,
                    

                }).ToList();

                var topLevelCategories = data.Where(c => c.ParentCategoryId == null).OrderBy(p => p.CategoryOrder).ToList();

                // Her bir üst seviye kategori için alt kategorileri al
                foreach (var category in topLevelCategories)
                {
                    category.SubCategories = GetSubCategories(data, category.ID).OrderBy(p => p.CategoryOrder).ToList();
                }

                // Response oluştur
                var response = new GetAllCategoryResponse {
                    Categories = topLevelCategories,
                    Message="Başarıyla Kategoriler gönderildi",
                    HassError=false,
                    StatusCode=HttpStatusCode.OK,
                    StatusCodeString=HttpStatusCode.OK.ToString(),
                    ErrorMessage=""
                };

                return response;
            }
            catch (Exception ex)
            {
                var response = new GetAllCategoryResponse
                {
                    Categories = null,
                    Message = "Kategoriler oluşturulurken bir hata ile karşılaşıldı.",
                    HassError = true,
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                    ErrorMessage = "Alınan hatanın kodu" + ex.Message.ToString()
                };
                return response;
            }
        }

        private IEnumerable<Category> GetSubCategories(List<Category> categories, Guid? parentId)
        {
            var subCategories = categories.Where(c => c.ParentCategoryId == parentId);
            foreach (var category in subCategories)
            {
                category.SubCategories = GetSubCategories(categories, category.ID).ToList();
            }
            return subCategories;
        }


    }
}