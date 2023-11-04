using Microsoft.AspNetCore.Builder;
using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR
{
    public static class HubRegistration
    {
        //Burada program.cs içinde gerekli olan kayıtları yapacağız.

        /*
         Son olarak yapmamız gereken bu configürasyonda yani api katmanında program.cs içerisinde 
        Hubları tanımlamamız gerekmekte.
            Bunları program.cs içinde yapmaktansa yani her bir hub için program.cs içinde tek tek
                tanımlama yapıp karmaşıklık yaratmaktansa burada böyle bir yöntem deneyerek gerekli düzenlemleri
            yapacağız.
            
        Burada diyoruz ki , 
                    
            webApplication.MapHub<a>("/b")
                    
                a=> Kardeşim sana bir hub gelecek gelen bu hup şu türde .
                b=> Gelen bu hub için verileri b endpoint'i üzerinden karşıla.
            
            
         */
        public static void MapHubs(this WebApplication webApplication)
        {
            webApplication.MapHub<ProductHub>("/product-hub");
        }
    }
}
