using System;
namespace OnionArch.Domain.Entities.Common
{
	public class BaseEntity
	{
		public Guid ID { get; set; }
		public DateTime CreateTime { get; set; }
		//Burada bulunan entity'nin her kalıtım alan Entity'de gözükmesini
		//istemeyebiliriz bu gibi durumlarda bu props'un değerini
		//virtual olarak ayarlamamız gerkemekte bu sayede
		//bunu override edebilir her yerde gözükmemesini notmap'ile ayralayabiliriz.
         virtual public DateTime UpdateTime { get; set; }
    }
}

