using Microsoft.Extensions.DependencyInjection;
using OnionArch.Application.Abstractions.HubServices;
using SignalR.HubServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR
{
    public static class ServicesRegistration
    {
        //extension fonksiyonumuz IOC içine tanımlamak için
        public static void AddSignalRServices(this IServiceCollection collection)
        {

            /* 
             
             AddTransient, AddScoped, ve AddSingleton, ASP.NET Core ve Dependency Injection (DI) konteynerlerinde hizmetleri (servisleri) kaydetmek için kullanılan metotlardır. 
                Bu metotlar, hizmetlerin nasıl ömrüne sahip olacağını belirler ve bu, bir uygulamanın davranışını büyük ölçüde etkileyebilir.

            AddTransient: 
                Bu metot, her bir istemci isteği için yeni bir hizmet örneği oluşturur. 
                Her istemci isteği için ayrı örnekler oluşturulduğu için, hizmetler kısa ömürlüdür. 
                Bu, hizmetin her kullanımında yeni bir nesnenin oluşturulmasını gerektiren senaryolarda kullanışlıdır.

            AddScoped:
                Bu metot, her bir HTTP isteği için bir hizmet örneği oluşturur. 
                İsteğin süresi boyunca aynı hizmet örneğini kullanır ve istek sona erdiğinde hizmet örneği atılır. 
                Bu, aynı istek içinde farklı bileşenlerin aynı hizmet örneğini paylaşması gereken senaryolarda kullanışlıdır.


            AddSingleton: 
                Bu metot, uygulamanın yaşam döngüsü boyunca yalnızca bir tane hizmet örneği oluşturur. 
                İlk talepten itibaren bu örnek oluşturulur ve uygulama kapatılana kadar yaşar. 
                Bu, bir hizmetin uygulama genelinde paylaşılması gereken durumlarda kullanılır.

           */
            collection.AddTransient<IProductHubService, ProductHubService>();
            collection.AddTransient<IOrderHubService, OrderHubService>();
            collection.AddSignalR();
        }
    }
}
