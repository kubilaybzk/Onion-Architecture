using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.View_Models.Category
{
    public class VM_Result_CategoryList
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryLinkTitle { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public bool CategoryHasTitleImage { get; set; }
        public string CategorySlug { get; set; }
        public bool CategoryDisplayStatus { get; set; }
        public int CategoryOrder { get; set; } 
        public string MaterializedPath { get; set; } 
        public string MaterializedPathByName { get; set; } 
        public string MaterializedPathBySlug { get; set; } 
        public Boolean IsSpecialCategory { get; set; } 
        public Boolean IsCampanyCategory { get; set; }
        public List<VM_Result_CategoryList>? SubCategories { get; set; } // List<Category> olarak değiştirildi
        public ICollection<CategoryImageFile>? CategoryImageFiles { get; set; }
        public ICollection<CategoryAttribute>? CategoryAttributes { get; set; }
    }
}
