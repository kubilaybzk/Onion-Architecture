using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Abstractions.HubServices
{
    //Burası bizim ProductHubService'in interface değerlerini yani abstraction mantığını oluşturuyoruz.
        //Bu sayede Dependency Injection sayesined istediğimiz her yerde buna erişebileceğiz.
            //Yada bu sayede sözleşmemiz dışında hiçbir şey yapılmasını izin vermeyeceğiz.
    public interface IProductHubService
    {
        //Şimdi varsayım olarak burada birkaç adet fonksiyon geliştirelim.
            //Ürün eklendiği zaman client tarafına bir mesaj gönderecek olan fonksiyon.
        Task ProductAddOperationMessage(string message);
    }
}
