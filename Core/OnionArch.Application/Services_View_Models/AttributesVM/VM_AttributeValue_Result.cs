using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Services_View_Models.AttributesVM
{
    public class VM_AttributeValue_Result
    {
        public Guid ID { get; set; }
        public Guid AttributeId { get; set; }
        public Domain.Entities.Attribute? Attribute { get; set; }
        public string Value { get; set; } // Özellik değeri (örneğin: Kırmızı, Deri)
        public ICollection<ProductAttribute>? ProductAttributes { get; set; }
    }
}
