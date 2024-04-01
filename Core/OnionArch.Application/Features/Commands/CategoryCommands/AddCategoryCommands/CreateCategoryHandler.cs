using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.CategoryCommands.AddCategoryCommands
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryRequest, CreateCategoryResponse>
    {
        private readonly IStorageService _storageService;
        public readonly ICategoryWriteRepository _categoryWriteRepository;

        public CreateCategoryHandler(ICategoryWriteRepository categoryWriteRepository, IStorageService storageService)
        {
            _categoryWriteRepository = categoryWriteRepository;
            _storageService = storageService;
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
                    CategorySlug = GenerateSlug(request.CategorySlug),
                    CategoryDisplayStatus = request.CategoryDisplayStatus,
                    CategoryLinkTitle = request.CategoryLinkTitle,
                    CategoryOrder = request.CategoryOrder,
                };

                if (request.CategoryHeaderImage != null)
                {
                    var result = await _storageService.UploadAsync("Category_Images", request.CategoryHeaderImage);
                    category.CategoryImageFiles = result.Select((d, index) => new CategoryImageFile
                    {
                        FileName = d.fileName,
                        Path = d.PathOrContainerName,
                        Storage = _storageService.StorageType,
                        Title = request.CategoryHeaderImage[index].FileName,
                        IsHeaderImage = request.CategoryHasTitleImage,
                        ShowImage = request.CategoryHasTitleImage,

                    }).ToList();
                }

                //if (request.ImageInfos != null && request.ImageInfos.Any())
                //{
                //    foreach (var imageInfo in request.ImageInfos)
                //    {
                //        var imageResult = await _storageService.UploadAsync("Category_Images", imageInfo.ImageFile);

                //        category.CategoryImageFiles.Add(new CategoryImageFile
                //        {
                //            FileName = imageResult.First().fileName,
                //            Path = imageResult.First().PathOrContainerName,
                //            Storage = _storageService.StorageType,
                //            Title = imageInfo.ImageAltTitle,
                //            IsHeaderImage = imageInfo.IsCategoryImage,
                //            ShowImage = imageInfo.ImageDisplayStatus
                //        });
                //    }
                //}

                await _categoryWriteRepository.AddAsync(category);
                await _categoryWriteRepository.SaveAsync();
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


        public static string RemoveTurkishCharacters(string input)
        {
            // Türkçe karakterleri çevirme
            input = input.Replace("ı", "i").Replace("İ", "I")
                         .Replace("ş", "s").Replace("Ş", "S")
                         .Replace("ğ", "g").Replace("Ğ", "G")
                         .Replace("ç", "c").Replace("Ç", "C")
                         .Replace("ö", "o").Replace("Ö", "O")
                         .Replace("ü", "u").Replace("Ü", "U");

            return input;
        }

        public static string GenerateSlug(string phrase)
        {
            // Türkçe karakterleri çıkar
            string str = RemoveTurkishCharacters(phrase).ToLower();

            // Geçersiz karakterleri temizle
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // Birden fazla boşluğu tek boşluğa dönüştür
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // 45 karakteri aşmayacak şekilde kırp ve boşlukları kes
            str = str.Substring(0, Math.Min(str.Length, 45)).Trim();
            // Boşlukları tireye dönüştür
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }

        public static void Main(string[] args)
        {
            string input = "Türkçe karakterler i ö ü ğ ç ş Ğ İ Ü Ö Ç Ş";
            string slug = GenerateSlug(input);
            Console.WriteLine(slug); // Output: turkce-karakterler-i-o-u-g-c-s-g-i-u-o-c-s
        }


    }



}
