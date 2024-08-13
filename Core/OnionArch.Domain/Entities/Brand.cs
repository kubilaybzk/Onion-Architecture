using OnionArch.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Entities
{
    public class Brand:BaseEntity
    {
        public string  BrandName { get; set; }
        public string  BrandSlug { get; set; }
        public string DetailTitle { get; set; }
        public string DetailDescription { get; set; }
        public Boolean isActive { get; set; }
        public int TotalProductCount { get; set; } = 0;

        //Seo ile ilgili alanlar.
        public string SeoLinkTitle { get; set; }
        public string SeoLinkDescription { get; set; }
        public string SeoDetailTitle { get; set; }
        public string SeoDetailDescription { get; set; }

        //Relation and Images
        public ICollection<Product> Products { get; set; }
        public BrandImageFile? BrandLogo { get; set; }

    }
}
