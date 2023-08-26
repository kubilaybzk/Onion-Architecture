using MediatR;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Features.Commands.Product.CreateOneProductWithImage;
using OnionArch.Domain.Entities;

public class CreateOneProductWithImageHandle: IRequestHandler<CreateOneProductWithImageRequest,CreateOneProductWithImageResponse>
{
    private readonly IStorageService _storageService;
    private readonly IProductWriteRepository _productWriteRepository;

    public CreateOneProductWithImageHandle(IStorageService storageService, IProductWriteRepository productWriteRepository)
    {
        _storageService = storageService;
        _productWriteRepository = productWriteRepository;
    }

    public async Task<CreateOneProductWithImageResponse> Handle(CreateOneProductWithImageRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _storageService.UploadAsync("product-images", request.ImageFiles);

            var product = new Product
            {
                Price = (float)request.Price,
                Name = request.Name,
                Stock = (short)request.Stock,
                ProductImageFiles = result.Select(d => new ProductImageFile
                {
                    FileName = d.fileName,
                    Path = d.PathOrContainerName,
                    Storage = _storageService.StorageType
                }).ToList()
            };

            await _productWriteRepository.AddAsync(product);
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
            return new CreateOneProductWithImageResponse
            {
                StatusCode = 500,
                Message =  ex.Message.ToString(),
                CreatedProduct = null
            };
        }

    }
}
