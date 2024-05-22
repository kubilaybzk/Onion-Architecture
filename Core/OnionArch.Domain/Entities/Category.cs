using OnionArch.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Entities
{
    public class Category:BaseEntity
    {
        public string CategoryName { get; set; }
        public string CategoryLinkTitle { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public bool CategoryHasTitleImage { get; set; }
        public string CategorySlug { get; set; }
        public bool CategoryDisplayStatus { get; set; }
        public List<Category> SubCategories { get; set; } // List<Category> olarak değiştirildi
        public ICollection<CategoryImageFile> CategoryImageFiles { get; set; }
        public int CategoryOrder { get; set; } = 0;
        public string MaterializedPath { get; set; } = "";
        public Boolean IsSpecialCategory { get; set; } = false;
        public Boolean IsCampanyCategory { get; set; } = false;
        public ICollection<Product> Products { get; set; }

    }
}
