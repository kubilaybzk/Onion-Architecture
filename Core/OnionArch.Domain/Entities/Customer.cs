using System;
using OnionArch.Domain.Entities.Common;

namespace OnionArch.Domain.Entities
{
	public class Customer:BaseEntity
	{

        public string Name { get; set; }

        public ICollection<Order> Orders { get; set; }
        
    }
}

