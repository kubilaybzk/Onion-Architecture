using OnionArch.Application.View_Models.BasketItem;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Abstractions.BasketServices
{
    /*
     Burası bizim basket ile ilgili işlemleri operasyonları modelleyeceğimiz alan.
         Mesela tüm itemleri görmek isteyebiliriz.
         İtemlerin sayılarını yani sepette kaç tane olduğunu düzenleyebiliriz.
         Baskette bulunan itemlerden kaldırma yapabiliriz.
    */
    public interface IBasketService
    {
        public Task<List<BasketItem>> GetBasketItemsAsync();
        public Task AddBasketItemToBasketAsync(VM_Add_BasketItem addBasketItem);

        public Task UpdateBasketItemAsync(VM_Update_BasketItem updateBasketItem);

        public Task RemoveBasketItemAsync(string id);




    }
}
