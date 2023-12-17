using System;
using OnionArch.Domain.Entities.Common;

namespace OnionArch.Domain.Entities
{
	public class Order: BaseEntity
    {
		public string Description { get; set; }
		public string Adress { get; set; }
        //Bir sipariş birden fazla ürün içerebilecek - Many-to-many relation
        public ICollection<Product> Products { get; set; }
        //Bir siparişin sadece tek bir müşteriye ait olabilir.
        public Customer Customer { get; set; }

        public Basket Basket { get; set; }

        public Guid BasketId { get; set; }
    }
}

