using OnionArch.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Entities
{
    public class HeroSectionSlider:BaseEntity
    {
        public string ImageAltTile { get; set; }
        public string ImageRederictLink { get; set; }
        public string ImageRedirectLinkTitle { get; set; }
        public int Order { get; set; }
        public ICollection<HeroSectionImage> HeroSectionImages { get; set; }

    }
}
