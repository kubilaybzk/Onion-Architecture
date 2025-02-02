using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Entities
{
    public class HeroSectionImage:File
    {
        public bool isSliderImage { get; set; }

        public HeroSectionSlider HeroSectionSlider { get; set; }
    }
}
