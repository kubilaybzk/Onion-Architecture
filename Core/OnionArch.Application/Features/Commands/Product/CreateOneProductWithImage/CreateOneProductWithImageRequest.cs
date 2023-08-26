using System;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace OnionArch.Application.Features.Commands.Product.CreateOneProductWithImage
{
	public class CreateOneProductWithImageRequest: IRequest<CreateOneProductWithImageResponse>
    {
        
        public decimal Price { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public IFormFileCollection? ImageFiles { get; set; }
    }
}

