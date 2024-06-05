using OnionArch.Application.GlobalResponse;
using System;
using p = OnionArch.Domain.Entities;
namespace OnionArch.Application.Features.Commands.Product.UpdateOneProduct
{
    public class UpdateOneProductResponse:GlobalResponseResult
    {
        public object Product { get; set; }
        public bool isUpdated { get; set; }

    }
}

