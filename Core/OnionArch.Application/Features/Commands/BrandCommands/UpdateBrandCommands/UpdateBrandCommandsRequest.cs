using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.View_Models.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.BrandCommands.UpdateBrandCommands
{
    public class UpdateBrandCommandsRequest:IRequest<UpdateBrandCommandsResponse>
    {
        public string Id { get; set; }
        public string BrandName { get; set; }
        public string BrandSlug { get; set; }
        public string DetailTitle { get; set; }
        public string DetailDescription { get; set; }
        public Boolean isActive { get; set; }

        //Seo tarafı için 
        public string SeoLinkTitle { get; set; }
        public string SeoLinkDescription { get; set; }
        public string SeoDetailTitle { get; set; }
        public string SeoDetailDescription { get; set; }
        public string SeoImageAltInformation { get; set; }

        //Relation ve Upload işemleri için 
        public IFormFileCollection? BrandLogo { get; set; }
    }
}
