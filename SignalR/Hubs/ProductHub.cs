using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Hubs
{
    /*
    Her zaman olması gerektiği gibi bu oluşturduğumuz
     class'ın bir şeyden kalıtım alması geriyor değil mi ?  
            İşte bu işlemi Microsoft.AspNetCore içinde bulunan 
               hali hazıda bize sunulan Hub interfacelerinden yararlanarak 
               oluşturacağız. 
           (Projemiz Class proje olduğu için Nuget üzerinden kurulum yapmamız lazım.)
    */
    public class ProductHub:Hub
    {
    }
}
