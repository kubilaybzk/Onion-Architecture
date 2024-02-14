using OnionArch.Application.View_Models.Addresses;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Abstractions.AddressServices
{
    public interface IAddressService
    {
        public Task<List<Address>> GetUserAddressesAsync();
        public Task<bool> CreateAddressAsync(VM_Create_Address address);
        public Task<bool> UpdateAddressAsync(VM_Update_Address address);
        public Task<bool> DeleteAddressAsync(string id);

    }
}
