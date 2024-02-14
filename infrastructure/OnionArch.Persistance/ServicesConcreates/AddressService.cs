using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.AddressServices;
using OnionArch.Application.Repositories.AddressCrud;
using OnionArch.Application.View_Models.Addresses;
using OnionArch.Domain.Entities;
using OnionArch.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.ServicesConcreates
{
    public class AddressService : IAddressService
    {
        readonly IHttpContextAccessor _httpContextAccessor; //User'a erişebilmemiz için gerekli
        readonly UserManager<AppUser> _userManager; //.Net'in User işlemleri ile ilgili bizim yazdığımız dışında olan interface
        readonly IAddressReadRepository _addressReadRepository;
        readonly IAddressWriteRepository _addressWriteRepository;



        public AddressService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IAddressReadRepository addressReadRepository, IAddressWriteRepository addressWriteRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _addressReadRepository = addressReadRepository;
            _addressWriteRepository = addressWriteRepository;
        }

        private async Task<AppUser> CurrentUser()
        {
            // Şu anki HttpContext'ten kullanıcı adını al
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

            // Kullanıcı adının boş olup olmadığını kontrol et
            if (!string.IsNullOrEmpty(username))
            {
                // Kullanıcıyı al ve sepetlerini içeren bir sorgu yap
                AppUser? user = await _userManager.Users
                    .Include(u => u.Addresses)
                    .FirstOrDefaultAsync(u => u.UserName == username);

                return user;

            }
            else
            {

                // Kullanıcı adı boşsa veya nullsa, bir istisna fırlat
                throw new Exception("Kullanıcı bulunamadı.");
            }
        }

        public async Task<bool> CreateAddressAsync(VM_Create_Address address)
        {
            AppUser currentUser = await CurrentUser();
            Address newaddress = new Address();
            newaddress.AddressName = address.AddressName;
            newaddress.Country = address.Country;
            newaddress.City = address.City;
            newaddress.District = address.District;
            newaddress.Neighbourhood = address.Neighbourhood;
            newaddress.LongAddress = address.LongAddress;
            newaddress.PhoneNumber = address.PhoneNumber;
            newaddress.User = currentUser;
            newaddress.UserId = currentUser.Id;
            var result = await _addressWriteRepository.AddAsync(newaddress);
            await _addressWriteRepository.SaveAsync();
            return result;
        }

        public async Task<bool> DeleteAddressAsync(string id)
        {
            Address deleteAddress = await _addressReadRepository.GetByIdAsync(id);
            if(deleteAddress != null)
            {
                _addressWriteRepository.Remove(deleteAddress);
                await _addressWriteRepository.SaveAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

         public async Task<List<Address>> GetUserAddressesAsync()
        {
            AppUser currentUserInfo = await CurrentUser(); // CurrentUser fonksiyonunu nereden çağırdığınıza bağlı olarak değişebilir
            var userAddresses = currentUserInfo.Addresses.Select(a => new Address
            {
                AddressName = a.AddressName,
                Country = a.Country,
                City = a.City,
                District = a.District,
                Neighbourhood = a.Neighbourhood,
                LongAddress = a.LongAddress,
                PhoneNumber = a.PhoneNumber,
                CreateTime = a.CreateTime,
                UpdateTime = a.UpdateTime,
                ID = a.ID
            }).ToList();


            if (userAddresses.Count > 0)
            {
                return userAddresses;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateAddressAsync(VM_Update_Address address)
        {
            Address UpdatedAddres = await _addressReadRepository.GetByIdAsync(address.Id);
            if(UpdatedAddres != null)
            {
                UpdatedAddres.AddressName = address.AddressName;
                UpdatedAddres.Country = address.Country;
                UpdatedAddres.City = address.City;
                UpdatedAddres.District = address.District;
                UpdatedAddres.Neighbourhood = address.Neighbourhood;
                UpdatedAddres.LongAddress = address.LongAddress;
                UpdatedAddres.PhoneNumber = address.PhoneNumber;
                await _addressWriteRepository.SaveAsync();
                return true;
            }
            else
            {
                return false;
            }
            throw new NotImplementedException();
        }
    }
}
