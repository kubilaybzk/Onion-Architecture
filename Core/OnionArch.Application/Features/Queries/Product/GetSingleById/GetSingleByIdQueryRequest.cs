using System;
using MediatR;

namespace OnionArch.Application.Features.Queries.Product.GetSingleById
{
	public class GetSingleByIdQueryRequest: IRequest<GetSingleByIdQueryResponse>
    {
        public string  id { get; set; }
    }
}

