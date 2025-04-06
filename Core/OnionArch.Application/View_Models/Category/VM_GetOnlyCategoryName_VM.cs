using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.View_Models.Category
{
    public class VM_GetOnlyCategoryName_VM
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public List<VM_GetOnlyCategoryName_VM> SubCategories { get; set; }
        public Boolean IsSpecialCategory { get; set; }  ;
    }
}
