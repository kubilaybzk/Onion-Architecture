using System;
using OnionArch.Application.GlobalResponse;
using OnionArch.Application.View_Models.Product;
using OnionArch.Domain.Entities;
namespace OnionArch.Application.Features.Queries.Product.GetSingleById
{
	public class GetSingleByIdQueryResponse:GlobalResponseResult
	{
        public List<VM_Result_ProductLink> Products { get; set; }

    }
}

