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
        public string CategoryName { get; set; }
        public string CategoryLinkTitle { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public bool CategoryHasTitleImage { get; set; }
        public string CategorySlug { get; set; }
        public bool CategoryDisplayStatus { get; set; }
        public List<OnionArch.Domain.Entities.Category> SubCategories { get; set; } // List<Category> olarak değiştirildi
        public ICollection<CategoryImageFile> CategoryImageFiles { get; set; }
        public Guid ID { get; set; }
    }
}
