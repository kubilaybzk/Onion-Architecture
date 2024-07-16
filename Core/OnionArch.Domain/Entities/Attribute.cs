using OnionArch.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Entities
{
    public class Attribute : BaseEntity
    {
        public string Name { get; set; } // Özellik adı (örneğin: Renk, Kordon Cinsi
                                         // 
        public string NameForSlug { get; set; } // Özellik adı (örneğin: Renk, Kordon Cinsi)
        public ICollection<AttributeValue> AttributeValues { get; set; }
        public ICollection<CategoryAttribute> CategoryAttributes { get; set; }
    }

}
