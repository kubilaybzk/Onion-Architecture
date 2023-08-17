using System;
using OnionArch.Domain.Entities.Common;

namespace OnionArch.Domain.Entities
{
	public class Product:BaseEntity
	{
		public string Name { get; set; }
		public int Stock { get; set; }
		public float Price { get; set; }
		//Bir ürün birden fazla sipariş içerebilir Many-to-many relation

        public ICollection<Order> Orders { get; set; }

		public ICollection<ProductImageFile> ProductImageFiles { get; set; }

	}
}

