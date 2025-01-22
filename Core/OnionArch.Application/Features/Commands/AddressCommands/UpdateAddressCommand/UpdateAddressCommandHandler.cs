using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.AddressServices;
using OnionArch.Application.Repositories.AddressCrud;
using OnionArch.Application.View_Models.Addresses;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AddressCommands.UpdateAddressCommand
{
    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommandRequest, UpdateAddressCommandResponse>
    {
        public readonly IAddressService _addressService;
        public readonly IAddressWriteRepository _addressWriteRepository;
        public readonly IAddressReadRepository _addressReadRepository;

        public UpdateAddressCommandHandler(IAddressService addressService, IAddressWriteRepository addressWriteRepository, IAddressReadRepository addressReadRepository)
        {
            _addressService = addressService;
            _addressWriteRepository = addressWriteRepository;
            _addressReadRepository = addressReadRepository;
        }

        public async Task<UpdateAddressCommandResponse> Handle(UpdateAddressCommandRequest request, CancellationToken cancellationToken)
        {
            Address currentAddres= await _addressReadRepository.GetByIdAsync(request.AddressID);

            currentAddres.PhoneNumber = request.PhoneNumber;
            currentAddres.AddressName = request.AddressName;
            currentAddres.Neighbourhood = request.Neighbourhood;
            currentAddres.LongAddress = request.LongAddress;
            currentAddres.City = request.City;
            currentAddres.Country = request.Country;
            currentAddres.District = request.District;
            currentAddres.RecipientName = request.RecipientName;
            currentAddres.RecipientSurName = request.RecipientSurName;
            currentAddres.IsDefaultAddress = request.IsDefaultAddress;
            currentAddres.IsInstitutional = request.IsInstitutional;
            currentAddres.TaxIdentificationNumber = request.TaxIdentificationNumber;
            currentAddres.TaxOffice = request.TaxOffice;
            currentAddres.CompanyName = request.CompanyName;
            currentAddres.ZipCode = request.ZipCode;
            try
            {
                int result = await _addressWriteRepository.SaveAsync();

                    if (result > 0)
                    {
                        return new()
                        {
                            Message = "Address Güncelleme başarılı",
                            StatusCode = HttpStatusCode.OK,
                            HassError = false,

                        };
                    }
                    else
                    {
                        return new()
                        {
                            Message = "Address Güncelleme başarısız",
                            StatusCode = HttpStatusCode.NotFound,
                            HassError = true,
                        };
                    }
            }
            catch (Exception ex)
            {
                return new()
                {
                    Message = "Addres güncelleme işlemi başarısız sunucu taraflı bir hata",
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                    HassError = true,
                    ErrorMessage = ex.Message
                };
            }

            

        }
    }
}
