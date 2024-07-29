using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnionArch.Application.Repositories.AttributeCrud.ProductAttributeCrud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Product.GetAllProductSlugs
{
    public class GetAllProductSlugsHandler : IRequestHandler<GetAllProductSlugsRequest, GetAllProductSlugsResponse>
    {
        private readonly IProductAttributeReadRepository _productReadRepository;
        private readonly ILogger<GetAllProductSlugsHandler> _logger;

        public GetAllProductSlugsHandler(IProductAttributeReadRepository productReadRepository, ILogger<GetAllProductSlugsHandler> logger)
        {
            _productReadRepository = productReadRepository;
            _logger = logger;
        }

        public async Task<GetAllProductSlugsResponse> Handle(GetAllProductSlugsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var allproducts = await _productReadRepository.Table.Select(p => p.Product.MaterializedProductPath).ToListAsync();
                _logger.LogInformation("GetAllProductSlugsResponse Çalıştı");
                Console.WriteLine("GetAllProductSlugsResponse Çalıştı");
                return new GetAllProductSlugsResponse()
                {
                    ProductSlugs = allproducts,
                    HassError = false,
                    ErrorMessage = "",
                    Message = "Slug verileri başarıyla gönderildi",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAllProductSlugsResponse Çalıştı ve Error verdi");
                Console.WriteLine("GetAllProductSlugsResponse Çalıştı ve Error verdi");
                return new GetAllProductSlugsResponse()
                {
                    ProductSlugs = null,
                    HassError = true,
                    ErrorMessage = "",
                    Message = "Slug verileri gönderiliken hata alındı ",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusCodeString = System.Net.HttpStatusCode.OK.ToString(),
                };
            }
             
        }
    }
}
