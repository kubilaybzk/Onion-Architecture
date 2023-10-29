using System;
using System.Text.Json.Serialization;

namespace OnionArch.Domain.Entities
{
	public class ProductImageFile:File
	{
        [JsonIgnore] 
		public ICollection<Product> Products { get; set; }


	}
}

