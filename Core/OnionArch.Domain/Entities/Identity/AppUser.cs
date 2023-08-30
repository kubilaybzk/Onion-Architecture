
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OnionArch.Domain.Entities.Identity
{
    //Burada generic olarak diyoruz ki senin identity özelliğine sahip olan kısmın string yani guid tipide olsun.
    public class AppUser: IdentityUser
    {
        //Burası tablomuza özel olarak ekleyeceğimiz alanlarımızı tasarladğımız alan.

        public string NameSurname { get; set; }
    }
}

