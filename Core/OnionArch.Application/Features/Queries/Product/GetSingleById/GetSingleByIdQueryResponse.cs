using System;
using OnionArch.Application.GlobalResponse;
using OnionArch.Domain.Entities;
namespace OnionArch.Application.Features.Queries.Product.GetSingleById
{
	public class GetSingleByIdQueryResponse:GlobalResponseResult
	{
        public object Products { get; set; }

    }
}

