using System;
namespace OnionArch.Domain.Entities
{
	public class InvoiceFile:File
	{
		//Sipariş için ne kadar ücret olduğunu tutan bir bilgi olsun.
		public decimal Price { get; set; }
	}
}

