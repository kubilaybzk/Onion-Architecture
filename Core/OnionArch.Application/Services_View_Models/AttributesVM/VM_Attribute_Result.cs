using OnionArch.Application.GlobalResponse;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Services_View_Models.AttributesVM
{
    public class VM_Attribute_Result : GlobalResponseResult
    {
        public Guid ID { get; set; }
        public string Name { get; set; } // Özellik adı (örneğin: Renk, Kordon Cinsi)
        public ICollection<AttributeValue>? AttributeValues { get; set; }
        public ICollection<CategoryAttribute>? CategoryAttributes { get; set; }
    }
}
