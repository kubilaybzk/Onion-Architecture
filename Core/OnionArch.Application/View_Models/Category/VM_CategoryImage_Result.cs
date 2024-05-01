using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.View_Models.Category
{
    public class VM_CategoryImage_Result
    {
        public bool ShowImage { get; set; }
        public bool IsHeaderImage { get; set; }
        public string ImageTitle { get; set; }
        public string CategoryID { get; set; }
        public string CategoryRedirectLink { get; set; }
        public string CategoryRedirectLinkTitle { get; set; }
        public int CategoryImageOrder {  get; set; }
        public string ImagePath { get; set; }
    }
}
