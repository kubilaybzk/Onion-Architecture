using System;
using MediatR;

namespace OnionArch.Application.Features.Commands.Product.DeleteProductById
{
	public class DeleteProductByIdCommandsRequest: IRequest<DeleteProductByIdCommandsResponse>
    {
        //Mevcut olan yapının içinde bulunan parametrelerimizi burada belirtiyoruz.
        public string id { get; set; }
	}
}

