using MediatR;
using Microsoft.AspNetCore.Http;
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
        //public List<CategoryImageInfo>? ImageInfos { get; set; } // liste olarak resim bilgilerini içeren yeni bir özellik
        public Guid? ParentCategoryId { get; set; } = null;
        public bool CategoryHasTitleImage { get; set; }
        public string CategorySlug { get; set; }
        public bool CategoryDisplayStatus { get; set; }
        public IFormFileCollection? CategoryHeaderImage { get; set; }
        public int CategoryOrder { get; set; }
    }
}
