using OnionArch.Application.GlobalResponse;
using System;
using P=OnionArch.Domain.Entities;
namespace OnionArch.Application.Features.Commands.Product.CreateOneProductWithImage
{
	public class CreateOneProductWithImageResponse:GlobalResponseResult
	{

        public P.Product CreatedProduct { get; set; }
        public bool isCreated { get; set; }
    }
}

