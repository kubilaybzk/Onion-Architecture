using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Entities
{
    public class BrandImageFile : File
    {
        public Guid BrandId { get; set; }
        public bool Showcase { get; set; }
        public Brand Brand { get; set; }
        public string SeoImageAltInformation { get; set; }
    }

}
 
