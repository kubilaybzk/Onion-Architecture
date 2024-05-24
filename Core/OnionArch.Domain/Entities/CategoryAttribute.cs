using OnionArch.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Entities
{
    public class CategoryAttribute : BaseEntity
    {
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid AttributeId { get; set; }
        public Attribute Attribute { get; set; }
    }

}
