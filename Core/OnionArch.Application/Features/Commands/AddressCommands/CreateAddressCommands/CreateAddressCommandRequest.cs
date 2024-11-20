using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AddressCommands.CreateAddressCommands
{
    public class CreateAddressCommandRequest:IRequest<CreateAddressCommandResponse>
    {
        public string AddressName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Neighbourhood { get; set; }
        public string LongAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string RecipientName { get; set; }
        public string RecipientSurName { get; set; }
        public bool IsDefaultAddress { get; set; } = false;
        public bool? IsInstitutional { get; set; } //Kurumsal faturamı değil mi kontrol ediyoruz.
        public string? TaxIdentificationNumber { get; set; }
        public string? TaxOffice { get; set; }
        public string? CompanyName { get; set; }
    }
}
