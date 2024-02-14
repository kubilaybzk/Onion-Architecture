
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OnionArch.Domain.Entities.Identity
{
  
    public class AppUser: IdentityUser
    {
        //Burası tablomuza özel olarak ekleyeceğimiz alanlarımızı tasarladğımız alan.

        public string NameSurname { get; set; }


        //RefreshToken'ı tablo içinde tutacak olan değer.
        public string? RefreshToken { get; set; }

        /*
         RefreshToken'ın ne kadar süre geçerli olacağını
           tablo içinde tutacak olan değer.
         */
        public DateTime? RefreshTokenEndDate { get; set; }


        //Sepete ilişkisi için

        public ICollection<Basket> Baskets{ get; set; }

        public ICollection<Address> Addresses { get; set; }
    }
}

