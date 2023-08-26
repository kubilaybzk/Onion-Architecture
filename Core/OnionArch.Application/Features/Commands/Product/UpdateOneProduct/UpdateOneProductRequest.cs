using System;
using MediatR;
using OnionArch.Application.View_Models;

namespace OnionArch.Application.Features.Commands.Product.UpdateOneProduct
{
	public class UpdateOneProductRequest: IRequest<UpdateOneProductResponse>
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public int Stock { get; set; }

        public float Price { get; set; }
    }
}

