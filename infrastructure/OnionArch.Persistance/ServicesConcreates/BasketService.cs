using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.BasketServices;
using OnionArch.Application.Abstractions.OrderCrud;
using OnionArch.Application.Abstractions.UserServices;
using OnionArch.Application.Repositories.BasketCrud;
using OnionArch.Application.Repositories.BasketItemCrud;
using OnionArch.Application.View_Models.BasketItem;
using OnionArch.Domain.Entities;
using OnionArch.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.ServicesConcreates
{
    public class BasketService : IBasketService
    {

        readonly IHttpContextAccessor _httpContextAccessor; //User'a erişebilmemiz için gerekli
        readonly UserManager<AppUser> _userManager; //.Net'in User işlemleri ile ilgili bizim yazdığımız dışında olan interface
        readonly IOrderReadRepository _orderReadRepository;
        readonly IBasketWriteRepository _basketWriteRepository;
        readonly IBasketItemReadRepository _basketItemReadRepository;
        readonly IBasketItemWriteRepository _basketItemWriteRepository;
        readonly IBasketReadRepository _basketReadRepository;

        public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IOrderReadRepository orderReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketReadRepository basketReadRepository = null)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _orderReadRepository = orderReadRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketReadRepository = basketReadRepository;
        }



        // Şu anki kullanıcıyı bulan ve ilgili sepeti döndüren metot.
        private async Task<Basket> CurrentUserBasket()
        {
            // Şu anki HttpContext'ten kullanıcı adını al
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

            // Kullanıcı adının boş olup olmadığını kontrol et
            if (!string.IsNullOrEmpty(username))
            {
                // Kullanıcıyı al ve sepetlerini içeren bir sorgu yap
                AppUser? user = await _userManager.Users
                    .Include(u => u.Baskets)
                    .FirstOrDefaultAsync(u => u.UserName == username);

                // Sepetleri ve ilgili siparişleri birleştiren sorgu (left join)
                var _basket = from basket in user.Baskets
                              join order in _orderReadRepository.Table
                              on basket.ID equals order.ID into BasketOrders
                              from order in BasketOrders.DefaultIfEmpty()
                              select new
                              {
                                  Basket = basket,
                                  Order = order
                              };

                // Hedef sepet değişkenini başlat
                Basket? targetBasket = null;

                // Siparişi olmayan sepetler var mı diye kontrol et
                if (_basket.Any(b => b.Order is null))
                {
                    // İlk siparişi olmayan sepeti hedef sepet olarak ayarla
                    targetBasket = _basket.FirstOrDefault(b => b.Order is null)?.Basket;
                }
                else
                {
                    // Eğer tüm sepetlere sipariş verilmişse, yeni bir sepet oluştur ve kullanıcının sepetlerine ekle
                    targetBasket = new Basket();
                    user.Baskets.Add(targetBasket);
                }

                // Değişiklikleri sepet deposuna kaydet
                await _basketWriteRepository.SaveAsync();

                // Hedef sepeti döndür
                return targetBasket;
            }

            // Kullanıcı adı boşsa veya nullsa, bir istisna fırlat
            throw new Exception("Kullanıcı bulunamadı.");
        }


        public async Task AddBasketItemToBasketAsync(VM_Add_BasketItem addedBasketItem)
        {
            //Öncelikle kullanıcının basket bilgilerine erişelim.
            Basket userBasket = await CurrentUserBasket();

            if (userBasket != null)
            {
                //Eklenen eleman için öncelikle söyle bir sorgu oluşturalım.
                //Gelen baseket itemi bizim sepetimizde olan herhangi bir basket item ile aynı değere mi sahip ? 
                //Eğer bu koşul sağlanıyor ise ,sepete bu ürün var demek oluyor bu.

                BasketItem CheckHasSameProduct = await _basketItemReadRepository.GetSingleAsync(
                    bi => bi.BasketId == userBasket.ID && bi.ProductId == Guid.Parse(addedBasketItem.ProductId));

                //Bu şu anlama geliyor artık sepette daha önce eklenmiş böyle bir ürün bulunmakta.
                //Bunun değerini değiştirelim.
                if (CheckHasSameProduct != null)
                {
                    CheckHasSameProduct.Quantity = CheckHasSameProduct.Quantity + addedBasketItem.Quantity;
                }
                else
                {
                    //Sepete gelen ürün ile ilgili daha önce hiçbir kayıt oluşturulmamış ise ürünü doğrudan biz ekleyelim.
                    await _basketItemWriteRepository.AddAsync(
                        new BasketItem()
                        {
                            BasketId = userBasket.ID,
                            ProductId = Guid.Parse(addedBasketItem.ProductId),
                            Quantity = addedBasketItem.Quantity,
                        }
                        );
                }
                await _basketItemWriteRepository.SaveAsync();

            }

        }

        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            // Kullanıcının mevcut sepetini almak için asenkron bir metodun sonucunu bekleyelim.
            Basket? currentUserBasket = await CurrentUserBasket();

            // Veritabanından kullanıcının sepetini ve sepet öğelerini almak için repository kullanılıyor.
            Basket? currentUsersItems = await _basketReadRepository.Table
                .Include(b => b.BasketItems)        // Sepet içindeki öğeleri içeri al
                .ThenInclude(b => b.Product)       
                .ThenInclude(b=>b.ProductImageFiles)
                // Sepet öğeleri içindeki ürünleri içeri al
                .FirstOrDefaultAsync(b => b.ID == currentUserBasket.ID);

            // Eğer kullanıcının sepeti null değilse, sepet içindeki öğeleri liste olarak döndürelim.

            var result = currentUsersItems?.BasketItems;
            if (result != null)
            {
                foreach (var basketItem in result)
                {
                    // Eğer bir ürün varsa ve ProductImageFiles null değilse, bu ürünün resim dosyalarını doldur
                    if (basketItem.Product != null && basketItem.Product.ProductImageFiles != null)
                    {
                        basketItem.Product.ProductImageFiles = basketItem.Product.ProductImageFiles.ToList();
                    }
                }
            }
            return result.ToList();

        }

        public async Task RemoveBasketItemAsync(string id)
        {
            //Burada kullanıcının basket bilgilerine ihtiyacımız yok basket içi.
            BasketItem? checkBasketHasThisItem = await _basketItemReadRepository.GetByIdAsync(id);
            if (checkBasketHasThisItem != null)
            {
                _basketItemWriteRepository.Remove(checkBasketHasThisItem);
                await _basketItemWriteRepository.SaveAsync();
            }


        }

        public async Task UpdateBasketItemAsync(VM_Update_BasketItem updateBasketItem)
        {

            BasketItem currentBasket = await _basketItemReadRepository.GetByIdAsync(updateBasketItem.BasketItemId);

            if (currentBasket != null)
            {
                currentBasket.Quantity = updateBasketItem.Quantity;
                await _basketItemWriteRepository.SaveAsync();

            }

        }
    }
}
