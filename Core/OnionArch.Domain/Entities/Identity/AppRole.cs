using System;
using Microsoft.AspNetCore.Identity;

namespace OnionArch.Domain.Entities.Identity
{
	//Burada generic olarak diyoruz ki senin identity özelliğine sahip olan kısmın string yani guid tipide olsun.
	public class AppRole:IdentityRole<string>
	{
        //burada ise Kullanıcının rollerini tanımlayacağımız alan.

    }
}

