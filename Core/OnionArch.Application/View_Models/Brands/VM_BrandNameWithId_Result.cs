using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.View_Models.Brands
{
    public class VM_BrandNameWithId_Result
    {
        public Guid Id { get; set; }
        public string BrandName { get; set; }
        public string BrandSlug { get; set; }
    }
}
