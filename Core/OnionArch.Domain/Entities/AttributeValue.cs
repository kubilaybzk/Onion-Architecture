using OnionArch.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Entities
{
    public class AttributeValue : BaseEntity
    {
        public Guid AttributeId { get; set; }
        public Attribute Attribute { get; set; }
        public string Value { get; set; } // Özellik değeri (örneğin: Kırmızı, Deri)
        public string ValueForSlug { get; set; }
        public ICollection<ProductAttribute> ProductAttributes { get; set; }
    }

}
