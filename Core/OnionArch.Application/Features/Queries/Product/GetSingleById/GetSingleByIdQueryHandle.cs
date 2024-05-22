using System;
using MediatR;
using OnionArch.Application.Abstractions.ProductCrud;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace OnionArch.Application.Features.Queries.Product.GetSingleById
{
    public class GetSingleByIdQueryHandle : IRequestHandler<GetSingleByIdQueryRequest, GetSingleByIdQueryResponse>
    {
        readonly private IProductReadRepository _productReadRepository;

        public GetSingleByIdQueryHandle(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<GetSingleByIdQueryResponse> Handle(GetSingleByIdQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _productReadRepository.GetByIdAsync(request.id);
                var productImages = product.ProductImageFiles
               .Select(p => new
               {
                   p.Path,
                   p.FileName,
                   p.ID,
                   p.CreateTime,
                   p.UpdateTime
               }).ToList();

                var productResult = new
                {
                    product.Name,
                    //product.Price,
                    //product.Stock,
                    product.ID,
                    productImages,   
                    product.CreateTime,
                    product.UpdateTime
                };



                return new GetSingleByIdQueryResponse()
                {
                    Products = productResult,
                    ErrorMessage = "",
                    HassError = false,
                    Message = "Listeleme başarıyla gerçekleşmiştir.",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),

                   

                };
            }
            catch(Exception ex)
            {
                return new GetSingleByIdQueryResponse()
                {
                    ErrorMessage = ex.Message,
                    HassError = true,
                    Message = "Ekleme işlemi sırasında bir hata ile karşılaşıldı.",
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString(),
                };
            }
        }
    }
}

