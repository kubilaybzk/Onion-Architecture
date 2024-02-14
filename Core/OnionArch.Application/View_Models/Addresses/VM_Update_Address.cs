using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.View_Models.Addresses
{
    public class VM_Update_Address
    {
        public string Id { get; set; }
        public string AddressName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Neighbourhood { get; set; }
        public string LongAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}
