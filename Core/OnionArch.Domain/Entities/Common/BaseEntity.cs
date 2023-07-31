using System;
namespace OnionArch.Domain.Entities.Common
{
	public class BaseEntity
	{
		public Guid ID { get; set; }
		public DateTime CreateTime { get; set; }
	}
}

