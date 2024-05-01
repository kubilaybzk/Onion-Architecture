using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.Storage;
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
        private readonly IStorageService _storageService;
        public UpdateCategoryHandler(ICategoryReadRepository categoryReadRepository, ICategoryWriteRepository categoryWriteRepository, IStorageService storageService)
        {
            _categoryReadRepository = categoryReadRepository;
            _categoryWriteRepository = categoryWriteRepository;
            _storageService = storageService;
        }

        public async Task<UpdateCategoryResponse> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {

            try
            {
                // İlgili kategoriyi bul ve özelliklerini güncelle
                var targetCategory = _categoryReadRepository.Table.Include(c => c.CategoryImageFiles).FirstOrDefault(p => p.ID == Guid.Parse(request.ID));

                if (targetCategory == null)
                {
                    throw new Exception("Kategori bulunamadı");
                }

                targetCategory.CategorySlug = request.CategorySlug;
                targetCategory.CategoryName = request.CategoryName;
                targetCategory.CategoryLinkTitle = request.CategoryLinkTitle;
                targetCategory.ParentCategoryId = request.ParentCategoryId;
                targetCategory.CategoryHasTitleImage = request.CategoryHasTitleImage;
                targetCategory.CategoryDisplayStatus = request.CategoryDisplayStatus;
                targetCategory.CategoryOrder = request.CategoryOrder;



                if (request.ImageInfos != null)
                {

                    // request.ImageInfos içindeki kimlikleri bir HashSet'e topla
                    var incomingImageIds = new HashSet<Guid>(request.ImageInfos.Select(imageInfo => Guid.Parse(imageInfo.Id)));

                    // Mevcut görüntü dosyalarını kontrol et ve silinecek resimleri topla
                    var imagesToRemove = targetCategory.CategoryImageFiles
                        .Where(imageFile => !incomingImageIds.Contains(imageFile.ID))
                        .ToList();

                    // Silinecek resimleri ayrı ayrı kaldır
                    foreach (var imageToRemove in imagesToRemove)
                    {
                        if (imageToRemove.IsHeaderImage==false)
                        {
                            targetCategory.CategoryHasTitleImage = false;
                        }
                        targetCategory.CategoryImageFiles.Remove(imageToRemove);
                    }

                    // Resimleri güncelle veya ekle
                    foreach (var imageInfo in request.ImageInfos)
                    {
                        var imageId = Guid.Parse(imageInfo.Id);

                        var existingImage = targetCategory.CategoryImageFiles.FirstOrDefault(images => images.ID == imageId);

                        if (imageInfo.Id == "00000000-0000-0000-0000-000000000000")
                        {
                            // Yeni resim ekleme
                            var imagerResult = await _storageService.UploadAsync("Category_Images", imageInfo.BannerImageFile);

                            targetCategory.CategoryImageFiles.Add(new CategoryImageFile
                            {
                                FileName = imagerResult.First().fileName,
                                Path = imagerResult.First().PathOrContainerName,
                                Storage = _storageService.StorageType,
                                ImageTitle = imageInfo.BannerImageTitle,
                                IsHeaderImage = true,
                                ShowImage = imageInfo.ShowImageOnBanner,
                                CategoryRedirectLink = imageInfo.BannerRedirectLink,
                                CategoryImageOrder = imageInfo.BannerImageOrder,
                                CategoryRedirectLinkTitle = imageInfo.BannerRedirectLinkTitle
                            });
                        }
                        else if (existingImage != null)
                        {
                            // Mevcut resimleri güncelle
                            existingImage.ImageTitle = imageInfo.BannerImageTitle;
                            existingImage.ShowImage = imageInfo.ShowImageOnBanner;
                            existingImage.CategoryRedirectLink = imageInfo.BannerRedirectLink;
                            existingImage.CategoryRedirectLinkTitle = imageInfo.BannerRedirectLinkTitle;
                            existingImage.CategoryImageOrder = imageInfo.BannerImageOrder;
                        }
                    }

                   

                }
                else
                {
                    if(targetCategory.CategoryImageFiles != null && targetCategory.CategoryImageFiles.Count>0)
                    {
                        List<CategoryImageFile> incomingImageIds  = targetCategory.CategoryImageFiles.ToList();

                        foreach (var imageToRemove in incomingImageIds)
                        {
                            if (imageToRemove.IsHeaderImage == false)
                            {
                                targetCategory.CategoryHasTitleImage = false;
                            }
                            targetCategory.CategoryImageFiles.Remove(imageToRemove);
                        }

                    }
                }

                if (request.CategoryHeaderImage != null)
                {
                    var result = await _storageService.UploadAsync("Category_Images", request.CategoryHeaderImage);
                   
                    var imagesToRemove = targetCategory.CategoryImageFiles
                      .Where(imageFile => imageFile.IsHeaderImage==false)
                      .ToList();

                   foreach(var remove in imagesToRemove)
                    {
                        targetCategory.CategoryImageFiles.Remove(remove);
                    }

                    foreach (var d in result)
                    {
                        var newImage = new CategoryImageFile
                        {
                            FileName = d.fileName,
                            Path = d.PathOrContainerName,
                            Storage = _storageService.StorageType,
                            ImageTitle = d.fileName,
                            IsHeaderImage = false,
                            ShowImage = request.CategoryDisplayStatus,
                        };
                        targetCategory.CategoryHasTitleImage=true;
                        targetCategory.CategoryImageFiles.Add(newImage);
                    }




                }







                await _categoryWriteRepository.SaveAsync();
                

                return new UpdateCategoryResponse
                {
                    ErrorMessage = null,
                    HassError = false,
                    Message = "Güncelleme işlemi başarılı",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = System.Net.HttpStatusCode.OK.ToString()
                };
            }
            catch (Exception ex)
            {
                return new UpdateCategoryResponse
                {
                    ErrorMessage = ex.Message,
                    HassError = true,
                    Message = "Bir hata oluştu",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString()
                };
            }


        }
    }
}
