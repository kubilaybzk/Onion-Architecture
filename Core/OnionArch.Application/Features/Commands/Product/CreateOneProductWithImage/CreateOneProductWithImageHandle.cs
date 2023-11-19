using MediatR;
using Microsoft.Extensions.Logging;
using OnionArch.Application.Abstractions.HubServices;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Features.Commands.Product.CreateOneProductWithImage;
using OnionArch.Domain.Entities;

public class CreateOneProductWithImageHandle: IRequestHandler<CreateOneProductWithImageRequest,CreateOneProductWithImageResponse>
{
    private readonly IStorageService _storageService;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly ILogger<CreateOneProductWithImageHandle> _logger;
    private readonly IProductHubService _productHubService;
    public CreateOneProductWithImageHandle(IStorageService storageService, 
                                            ILogger<CreateOneProductWithImageHandle> logger,
                                            IProductHubService productHubService,
                                            IProductWriteRepository productWriteRepository)

    {
        _storageService = storageService;
        _productWriteRepository = productWriteRepository;
        _logger = logger;
        _productHubService= productHubService;


    }

    public async Task<CreateOneProductWithImageResponse> Handle(CreateOneProductWithImageRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _storageService.UploadAsync("product-images", request.ImageFiles);
            Console.Write(result);

            
            var product = new Product
            {
                Price = (float)request.Price,
                Name = request.Name,
                Stock = (short)request.Stock,
                ProductImageFiles = result.Select((d,index) => new ProductImageFile
                {
                    FileName = d.fileName,
                    Path = d.PathOrContainerName,
                    Storage = _storageService.StorageType,
                    Showcase=(index==0),

                }).ToList()
            };




            await _productWriteRepository.AddAsync(product);
            _logger.LogInformation("Başarılı bir şekilde ürün eklendi");
            await _productHubService.ProductAddOperationMessage("Ürün listesine bir adet ürün eklendi");
            await _productWriteRepository.SaveAsync();
            

            return new CreateOneProductWithImageResponse
            {
                StatusCode = 201,
                Message = "Ürün başarıyla eklendi.",
                CreatedProduct = product
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ürün eklenirken bir sorun ile karşılaşıldı: {ex.Message}");
            return new CreateOneProductWithImageResponse
            {
                StatusCode = 500,
                Message =  ex.Message.ToString(),
                CreatedProduct = null
            };
        }

    }
}
