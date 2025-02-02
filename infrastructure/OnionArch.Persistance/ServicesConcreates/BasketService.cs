using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.BasketServices;
using OnionArch.Application.Abstractions.OrderCrud;
using OnionArch.Application.Repositories.BasketCrud;
using OnionArch.Application.Repositories.BasketItemCrud;
using OnionArch.Application.View_Models.BasketItem;
using OnionArch.Domain.Entities;
using OnionArch.Domain.Entities.Identity;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.ServicesConcreates
{
    public class BasketService : IBasketService
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly UserManager<AppUser> _userManager;
        readonly IOrderReadRepository _orderReadRepository;
        readonly IBasketWriteRepository _basketWriteRepository;
        readonly IBasketItemReadRepository _basketItemReadRepository;
        readonly IBasketItemWriteRepository _basketItemWriteRepository;
        readonly IBasketReadRepository _basketReadRepository;
        private readonly OnionArchDBContext _context;

        public BasketService(IHttpContextAccessor httpContextAccessor,
            UserManager<AppUser> userManager,
            IOrderReadRepository orderReadRepository,
            IBasketWriteRepository basketWriteRepository,
            IBasketItemReadRepository basketItemReadRepository,
            IBasketItemWriteRepository basketItemWriteRepository,
            IBasketReadRepository basketReadRepository = null,
            OnionArchDBContext context = null)
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

        public async Task<Basket> CurrentUserBasket()
        {
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new Exception("Kullanıcı bulunamadı");

            var userWithBaskets = await _basketWriteRepository.Table
                .Include(b => b.BasketItems)
                    .ThenInclude(bi => bi.Product)
                .Include(b => b.Order)
                .Include(b => b.User)
                .AsSplitQuery()
                .Where(b => b.User.UserName == username)
                .ToListAsync();

            if (!userWithBaskets.Any())
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                    throw new Exception("Kullanıcı bulunamadı");

                var newBasket = new Basket
                {
                    User = user,
                    TotalBasketAmount = 0
                };

                await _basketWriteRepository.AddAsync(newBasket);
                await _basketWriteRepository.SaveAsync();
                return newBasket;
            }

            var activeBasket = userWithBaskets
                .FirstOrDefault(b => b.Order == null ||
                                   (b.Order != null && b.Order.isOrdered == false && b.Order.paidStatus == false))
                ?? new Basket();

            if (activeBasket.ID == Guid.Empty)
            {
                var user = userWithBaskets.First().User;
                activeBasket.User = user;
                await _basketWriteRepository.AddAsync(activeBasket);
                await _basketWriteRepository.SaveAsync();
            }

            return activeBasket;
        }

        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            var currentUserBasket = await CurrentUserBasket();

            var basketWithItems = await _basketReadRepository.Table
                .AsSplitQuery()
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

        public async Task<bool> AddBasketItemToBasketAsync(VM_Add_BasketItem addedBasketItem)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var userBasket = await CurrentUserBasket();
                if (userBasket == null)
                    return false;

                var existingItem = userBasket.BasketItems?
                    .FirstOrDefault(bi => bi.ProductId == Guid.Parse(addedBasketItem.ProductId));

                if (existingItem != null)
                {
                    existingItem.Quantity += addedBasketItem.Quantity;
                }
                else
                {
                    if (userBasket.BasketItems == null)
                        userBasket.BasketItems = new List<BasketItem>();

                    userBasket.BasketItems.Add(new BasketItem
                    {
                        BasketId = userBasket.ID,
                        ProductId = Guid.Parse(addedBasketItem.ProductId),
                        Quantity = addedBasketItem.Quantity
                    });
                }

                await _basketWriteRepository.SaveAsync();
                await TotalBasketAmountCalculatorAsync(userBasket);
                await _basketWriteRepository.SaveAsync(); // Total hesaplandıktan sonra tekrar kaydet
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Boolean> AddMultipleBasketItemsToBasketAsync(List<VM_Add_BasketItem> addedBasketItems)
        {
            try
            {
                var userBasket = await CurrentUserBasket();
                if (userBasket != null)
                {
                    if (userBasket.BasketItems == null)
                        userBasket.BasketItems = new List<BasketItem>();

                    foreach (var addedBasketItem in addedBasketItems)
                    {
                        var existingItem = userBasket.BasketItems
                            .FirstOrDefault(bi => bi.ProductId == Guid.Parse(addedBasketItem.ProductId));

                        if (existingItem != null)
                        {
                            existingItem.Quantity += addedBasketItem.Quantity;
                        }
                        else
                        {
                            userBasket.BasketItems.Add(new BasketItem
                            {
                                BasketId = userBasket.ID,
                                ProductId = Guid.Parse(addedBasketItem.ProductId),
                                Quantity = addedBasketItem.Quantity
                            });
                        }
                    }

                    await _basketWriteRepository.SaveAsync();
                    await TotalBasketAmountCalculatorAsync(userBasket);
                    await _basketWriteRepository.SaveAsync(); // Total hesaplandıktan sonra tekrar kaydet
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Boolean> RemoveBasketItemAsync(string id)
        {
            try
            {
                var userBasket = await CurrentUserBasket();

                var basketItem = userBasket.BasketItems?
                    .FirstOrDefault(bi => bi.ID == Guid.Parse(id));

                if (basketItem != null)
                {
                    _basketItemWriteRepository.Remove(basketItem);
                    await _basketItemWriteRepository.SaveAsync();
                    await TotalBasketAmountCalculatorAsync(userBasket);
                    await _basketWriteRepository.SaveAsync(); // Total hesaplandıktan sonra tekrar kaydet
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Boolean> UpdateBasketItemAsync(VM_Update_BasketItem updateBasketItem)
        {
            try
            {
                var userBasket = await CurrentUserBasket();

                var basketItem = userBasket.BasketItems?
                    .FirstOrDefault(bi => bi.ID == Guid.Parse(updateBasketItem.BasketItemId));

                if (basketItem != null)
                {
                    basketItem.Quantity = updateBasketItem.Quantity;
                    await _basketItemWriteRepository.SaveAsync();
                    await TotalBasketAmountCalculatorAsync(userBasket);
                    await _basketWriteRepository.SaveAsync(); // Total hesaplandıktan sonra tekrar kaydet
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ClearBasketAsync(Guid basketId)
        {
            try
            {
                var userBasket = await CurrentUserBasket();
                if (userBasket.ID != basketId)
                {
                    throw new Exception("Yetkisiz işlem");
                }

                if (userBasket.BasketItems != null)
                {
                    _basketItemWriteRepository.RemoveRange(userBasket.BasketItems?.ToList());
                    await _basketItemWriteRepository.SaveAsync();

                    userBasket.TotalBasketAmount = 0;
                    await _basketWriteRepository.SaveAsync();
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Basket> TotalBasketAmountCalculatorAsync(Basket basket)
        {
            try
            {
                if (basket == null)
                    throw new Exception("Sepet bulunamadı");

                decimal totalAmount = 0;

                // Her basket item için product bilgisini ayrı ayrı çekelim
                if (basket.BasketItems != null)
                {
                    foreach (var item in basket.BasketItems)
                    {
                        var product = await _context.Products
                            .FirstOrDefaultAsync(p => p.ID == item.ProductId);

                        if (product != null)
                        {
                            totalAmount += product.LastPrice * item.Quantity;
                        }
                    }
                }

                basket.TotalBasketAmount = totalAmount;
                return basket;
            }
            catch
            {
                throw new Exception("Basket toplama işleminde problem");
            }
        }
    }
}