using System;
namespace OnionArch.Domain.Entities
{
	public class ProductImageFile:File
	{
		public ICollection<Product> Products { get; set; }


	}
}

