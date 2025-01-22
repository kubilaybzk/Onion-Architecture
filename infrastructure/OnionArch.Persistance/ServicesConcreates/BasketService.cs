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
using OnionArch.Persistance.Contexts;
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
        private readonly OnionArchDBContext _context;

        public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IOrderReadRepository orderReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketReadRepository basketReadRepository = null, OnionArchDBContext context = null)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _orderReadRepository = orderReadRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketReadRepository = basketReadRepository;
            _context = context;
        }



        // Şu anki kullanıcıyı bulan ve ilgili sepeti döndüren metot.
        public async Task<Basket> CurrentUserBasket()
        {
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new Exception("Kullanıcı bulunamadı");

            // Aktif sepeti ve siparişleri birlikte sorgula
            var userWithBasket = await _userManager.Users
                .Include(u => u.Baskets)
                    .ThenInclude(b => b.Order)
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (userWithBasket == null)
                throw new Exception("Kullanıcı bulunamadı");

            // Aktif sepet kontrolü - siparişe dönüşmemiş VE ödeme yapılmamış sepet
            var activeBasket = userWithBasket.Baskets
                .FirstOrDefault(b => b.Order == null ||
                                   (b.Order != null && b.Order.isOrdered==false && b.Order.paidStatus==false))
                ?? new Basket();

            if (activeBasket.ID == Guid.Empty)
            {
                userWithBasket.Baskets.Add(activeBasket);
                await _basketWriteRepository.SaveAsync();
            }

            return activeBasket;
        }

        public async Task<bool> AddBasketItemToBasketAsync(VM_Add_BasketItem addedBasketItem)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var userBasket = await CurrentUserBasket();
                if (userBasket == null)
                    return false;

                var existingItem = await _basketItemReadRepository.GetSingleAsync(
                    bi => bi.BasketId == userBasket.ID && bi.ProductId == Guid.Parse(addedBasketItem.ProductId));

                if (existingItem != null)
                {
                    existingItem.Quantity += addedBasketItem.Quantity;
                }
                else
                {
                    await _basketItemWriteRepository.AddAsync(new BasketItem
                    {
                        BasketId = userBasket.ID,
                        ProductId = Guid.Parse(addedBasketItem.ProductId),
                        Quantity = addedBasketItem.Quantity,
                    });
                }

                await _basketItemWriteRepository.SaveAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            // Tek bir sorgu ile tüm verileri çekelim
            var currentUserBasket = await CurrentUserBasket();

            // Eager loading ile tek sorguda ilişkiliverileri çekelim
            var basketWithItems = await _basketReadRepository.Table
                .AsSplitQuery() // Büyük sorgularda performansı artırır
                .Include(b => b.BasketItems)
                    .ThenInclude(bi => bi.Product)
                        .ThenInclude(p => p.ProductImageFiles.Where(pif => pif.Showcase))
                .Include(b => b.BasketItems)
                    .ThenInclude(bi => bi.Product)
                        .ThenInclude(p => p.Brand)
                .Include(b => b.DiscountCoupon)
                .FirstOrDefaultAsync(b => b.ID == currentUserBasket.ID);

            return basketWithItems?.BasketItems?.ToList() ?? new List<BasketItem>();
        }

        public async Task<Boolean> RemoveBasketItemAsync(string id)
        {
            //Burada kullanıcının basket bilgilerine ihtiyacımız yok basket içi.
            try
            {
                BasketItem? checkBasketHasThisItem = await _basketItemReadRepository.GetByIdAsync(id);
                if (checkBasketHasThisItem != null)
                {
                    _basketItemWriteRepository.Remove(checkBasketHasThisItem);
                    await _basketItemWriteRepository.SaveAsync();

                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }


        }

        public async Task<Boolean> UpdateBasketItemAsync(VM_Update_BasketItem updateBasketItem)
        {

            try
            {
                BasketItem currentBasket = await _basketItemReadRepository.GetByIdAsync(updateBasketItem.BasketItemId);

                if (currentBasket != null)
                {
                    currentBasket.Quantity = updateBasketItem.Quantity;
                    await _basketItemWriteRepository.SaveAsync();

                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public async Task<Boolean> AddMultipleBasketItemsToBasketAsync(List<VM_Add_BasketItem> addedBasketItems)
        {
            Basket userBasket = await CurrentUserBasket();

            try
            {
                if (userBasket != null)
                {
                    foreach (var addedBasketItem in addedBasketItems)
                    {
                        BasketItem checkHasSameProduct = await _basketItemReadRepository.GetSingleAsync(
                            bi => bi.BasketId == userBasket.ID && bi.ProductId == Guid.Parse(addedBasketItem.ProductId));

                        if (checkHasSameProduct != null)
                        {
                            checkHasSameProduct.Quantity += addedBasketItem.Quantity;
                        }
                        else
                        {
                            await _basketItemWriteRepository.AddAsync(
                                new BasketItem
                                {
                                    BasketId = userBasket.ID,
                                    ProductId = Guid.Parse(addedBasketItem.ProductId),
                                    Quantity = addedBasketItem.Quantity,
                                });
                        }
                    }

                    await _basketItemWriteRepository.SaveAsync();

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ClearBasketAsync(Guid basketId)
        {
           
            try
            {
                var basketItems = await _basketItemReadRepository
                    .GetWhere(bi => bi.BasketId == basketId)
                    .ToListAsync();

                _basketItemWriteRepository.RemoveRange(basketItems);
                await _basketItemWriteRepository.SaveAsync();

               
                return true;
            }
            catch
            {
               
                throw;
            }
        }
    }
}
