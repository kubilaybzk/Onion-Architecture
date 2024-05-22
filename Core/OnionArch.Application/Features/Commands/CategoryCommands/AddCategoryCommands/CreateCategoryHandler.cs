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

        //public static string RemoveTurkishCharacters(string input)
        //{
        //    // Türkçe karakterleri çevirme
        //    input = input.Replace("ı", "i").Replace("İ", "I")
        //                 .Replace("ş", "s").Replace("Ş", "S")
        //                 .Replace("ğ", "g").Replace("Ğ", "G")
        //                 .Replace("ç", "c").Replace("Ç", "C")
        //                 .Replace("ö", "o").Replace("Ö", "O")
        //                 .Replace("ü", "u").Replace("Ü", "U");

        //    return input;
        //}

        //public static string GenerateSlug(string phrase)
        //{
        //    // Türkçe karakterleri çıkar
        //    string str = RemoveTurkishCharacters(phrase).ToLower();

        //    // Geçersiz karakterleri temizle
        //    str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        //    // Birden fazla boşluğu tek boşluğa dönüştür
        //    str = Regex.Replace(str, @"\s+", " ").Trim();
        //    // 45 karakteri aşmayacak şekilde kırp ve boşlukları kes
        //    str = str.Substring(0, Math.Min(str.Length, 45)).Trim();
        //    // Boşlukları tireye dönüştür
        //    str = Regex.Replace(str, @"\s", "-");

        //    return str;
        //}

        //public async Task<string> GenerateMaterializedPath(Guid? parentCategoryId, string categoryName)
        //{
        //    // Eğer bir üst kategori yoksa, malzeme kodu kategori adıyla başlar
        //    if (!parentCategoryId.HasValue)
        //    {
        //        return GenerateSlug(categoryName);
        //    }
        //    else
        //    {
        //        // Eğer bir üst kategori varsa, üst kategorinin malzeme kodunu alıp kendi adını ekler
        //        var parentCategory = await _categoryReadRepository.GetByIdAsync(parentCategoryId.ToString());
        //        return $"{parentCategory.MaterializedPath}.{GenerateSlug(categoryName)}";
        //    }

        //}
    }
}
