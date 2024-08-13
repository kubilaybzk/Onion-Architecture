using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.View_Models.Brands
{
    public class VM_Brand_Result
    {
        public Guid Id { get; set; }
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

        //Relation için 
        public VM_BrandImageFile_Result? BrandLogo { get; set; }
    }

    public class VM_BrandImageFile_Result
    {
        public bool Showcase { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Storage { get; set; }
        public string SeoImageAltInformation { get; set; }
    }

}
