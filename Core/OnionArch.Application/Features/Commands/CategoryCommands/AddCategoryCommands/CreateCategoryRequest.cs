using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.CategoryCommands.AddCategoryCommands
{
    public class CreateCategoryRequest : IRequest<CreateCategoryResponse>
    {
        public string CategoryName { get; set; }
        public string CategoryLinkTitle { get; set; }
        //public List<CategoryImageInfo>? ImageInfos { get; set; } // liste olarak resim bilgilerini içeren yeni bir özellik
        public Guid? ParentCategoryId { get; set; } = null;
        public bool CategoryHasTitleImage { get; set; }
        public string CategorySlug { get; set; }
        public bool CategoryDisplayStatus { get; set; }
        public IFormFileCollection? CategoryHeaderImage { get; set; }

        public int CategoryOrder { get; set; }

    }

    //public class CategoryImageInfo
    //{
    //    public IFormFileCollection ImageFile { get; set; }
    //    public string ImageAltTitle { get; set; }
    //    public string ImageRedirectUrl { get; set; }
    //    public bool ImageDisplayStatus { get; set; }
    //    public bool IsCategoryImage { get; set; }
    //}

}
