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
        public string ImageTitle { get; set; }
        public virtual ICollection<Category> CategoryInfo { get; set; }
        public string CategoryRedirectLink { get; set; }
        public string CategoryRedirectLinkTitle {  get; set; }
        public int CategoryImageOrder { get; set; } = 0;

    }
}
