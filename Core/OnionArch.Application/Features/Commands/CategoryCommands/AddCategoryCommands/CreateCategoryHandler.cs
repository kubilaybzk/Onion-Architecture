using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.CategoryServices;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.CategoryCommands.AddCategoryCommands
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryRequest, CreateCategoryResponse>
    {
        private readonly IStorageService _storageService;
        private readonly ICategoryWriteRepository _categoryWriteRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly ICategoryServices _categoryServices;


        public CreateCategoryHandler(ICategoryWriteRepository categoryWriteRepository, IStorageService storageService, ICategoryServices categoryServices)
        {
            _categoryWriteRepository = categoryWriteRepository;
            _storageService = storageService;
            _categoryServices = categoryServices;
        }

        public async Task<CreateCategoryResponse> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var category = new Category()
                {
                    CategoryName = request.CategoryName,
                    ParentCategoryId = request.ParentCategoryId,
                    CategoryHasTitleImage = request.CategoryHasTitleImage,
                    CategorySlug = _categoryServices.GenerateSlug(request.CategorySlug),
                    CategoryDisplayStatus = request.CategoryDisplayStatus,
                    CategoryLinkTitle = request.CategoryLinkTitle,
                    CategoryOrder = request.CategoryOrder,
                    MaterializedPath = "",
                    IsCampanyCategory = request.IsCampanyCategory,
                    IsSpecialCategory = request.IsSpecialCategory,
                    SubCategories= new List<Category>(),
                    MaterializedPathByName= request.CategoryName,
                    MaterializedPathBySlug=request.CategorySlug
                };

                //Add category's headerImage
                if (request.CategoryHeaderImage != null)
                {
                    var result = await _storageService.UploadAsync("Category_Images", request.CategoryHeaderImage);
                    category.CategoryImageFiles = result.Select((d, index) => new CategoryImageFile
                    {
                        FileName = d.fileName,
                        Path = d.PathOrContainerName,
                        Storage = _storageService.StorageType,
                        ImageTitle = request.CategoryHeaderImage[index].FileName,
                        IsHeaderImage = false,
                        ShowImage = request.CategoryDisplayStatus,
                        CategoryRedirectLink = request.CategoryName,
                        CategoryRedirectLinkTitle = request.CategoryLinkTitle,
                        CategoryImageOrder = 0
                    }).ToList();
                }
                //Add Category's bannerImage
                if (request.ImageInfos != null && request.ImageInfos.Any())
                {
                    foreach (var imageInfo in request.ImageInfos)
                    {
                        var imageResult = await _storageService.UploadAsync("Category_Images", imageInfo.BannerImageFile);

                        category.CategoryImageFiles.Add(new CategoryImageFile
                        {
                            FileName = imageResult.First().fileName,
                            Path = imageResult.First().PathOrContainerName,
                            Storage = _storageService.StorageType,
                            ImageTitle = imageInfo.BannerImageTitle,
                            IsHeaderImage = true,
                            ShowImage = imageInfo.ShowImageOnBanner,
                            CategoryRedirectLink = imageInfo.BannerRedirectLink,
                            CategoryImageOrder = imageInfo.BannerImageOrder,
                            CategoryRedirectLinkTitle = imageInfo.BannerRedirectLinkTitle
                        });
                    }
                }

                await _categoryWriteRepository.AddAsync(category);
                await _categoryWriteRepository.SaveAsync();

                if (request.ParentCategoryId != null)
                {
                    await _categoryServices.AddSubCategoryAsync(Guid.Parse(request.ParentCategoryId.ToString()), category, false);
                }
                    await _categoryServices.AssignMaterializedPathAsync(category, request.ParentCategoryId);

                return new CreateCategoryResponse()
                {
                    ErrorMessage = "",
                    HassError = false,
                    Message = "Ekleme başarıyla gerçekleşmiştir.",
                    StatusCode = System.Net.HttpStatusCode.Created,
                    StatusCodeString = System.Net.HttpStatusCode.Created.ToString(),
                };
            }
            catch (Exception ex)
            {
                return new CreateCategoryResponse()
                {
                    ErrorMessage = ex.Message.ToString(),
                    HassError = true,
                    Message = "Ekleme işlemi sırasında sunucu kaynaklı bir hata.",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString(),
                };
            }
        }

    }
}
