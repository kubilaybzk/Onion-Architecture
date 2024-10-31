using OnionArch.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Entities
{
    public class  BrandAttribute: BaseEntity
    {
        public Brand Brand { get; set; }
        public Guid BrandId { get; set; }
        public Guid FilterId { get; set; }
        public Attribute Filter { get; set; }
        public string FilterType { get; set; }
        public int Order { get; set; }
    }
}
