using OnionArch.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Entities
{
    public class CategoryImageFile:File
    {
        public bool ShowImage { get; set; }
        public bool IsHeaderImage { get; set; }
        public string Title { get; set; }
        public ICollection<Category> CategoryInfo { get; set; }

    }
}
