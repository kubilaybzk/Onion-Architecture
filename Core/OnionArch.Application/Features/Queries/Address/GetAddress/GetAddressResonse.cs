using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Address.GetAddress
{
    public class GetAddressResonse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<OnionArch.Domain.Entities.Address> Addresses { get; set; }
    }
}
