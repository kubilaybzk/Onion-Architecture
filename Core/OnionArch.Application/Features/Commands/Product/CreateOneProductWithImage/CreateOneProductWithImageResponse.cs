using System;
using P=OnionArch.Domain.Entities;
namespace OnionArch.Application.Features.Commands.Product.CreateOneProductWithImage
{
	public class CreateOneProductWithImageResponse
	{
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public P.Product CreatedProduct { get; set; }
    }
}

