using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Features.Commands.CategoryCommands.AddCategoryCommands;
using OnionArch.Application.Features.Commands.CategoryCommands.DeleteCategoryComands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.CategoryCommands.UpdateCategoryComands
{
    public class UpdateCategoryRequest: IRequest<UpdateCategoryResponse>
    {
        public string ID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryLinkTitle { get; set; }
        public List<CategoryImageInfoEdit>? ImageInfos { get; set; } // liste olarak resim bilgilerini içeren yeni bir özellik
        public Guid? ParentCategoryId { get; set; } = null;
        public bool CategoryHasTitleImage { get; set; }
        public string CategorySlug { get; set; }
        public bool CategoryDisplayStatus { get; set; }
        public IFormFileCollection? CategoryHeaderImage { get; set; }
        public int CategoryOrder { get; set; }
        public Boolean IsSpecialCategory { get; set; }
        public Boolean IsCampanyCategory { get; set; }
    }

    public class CategoryImageInfoEdit
    {
        public IFormFileCollection? BannerImageFile { get; set; }
        public bool ShowImageOnBanner { get; set; }
        public bool IsHeaderImage { get; set; }
        public string BannerImageTitle { get; set; }
        public string BannerRedirectLink { get; set; }
        public string BannerRedirectLinkTitle { get; set; }
        public int BannerImageOrder { get; set; } = 0;
        public string? Id { get; set; }  
    }


}
