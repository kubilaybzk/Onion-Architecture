using System;
using MediatR;
using OnionArch.Application.Abstractions.ProductCrud;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.View_Models.Product;

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
            GetSingleByIdQueryResponse result = new();
            try
            {
                var product = _productReadRepository.GetWhere(p => p.ID == Guid.Parse(request.id.ToString()))
                    .Include(p => p.ProductImageFiles);

              
                

                if (product == null)
                {
                    result.Products = null;
                    result.ErrorMessage = "";
                    result.HassError = false;
                    result.Message = "İsteğe uygun ürün bulunamadı.";
                    result.StatusCode = System.Net.HttpStatusCode.NotFound;
                    result.StatusCodeString = System.Net.HttpStatusCode.NotFound.ToString();
                }

                else
                {
                    var productresult = product.Select(p => new VM_Result_ProductLink()
                    {
                        AppliedDiscountPrice = p.DiscountPrice,
                        ProductImageFiles = p.ProductImageFiles.Select(p => new Domain.Entities.ProductImageFile()
                        {
                            ID = p.ID,
                            FileName = p.FileName,
                            Path = p.Path,
                            Showcase = p.Showcase,
                            Storage = p.Storage,
                        }).ToList(),
                        AppliedDiscountRate = p.DiscountRate,
                        Brand = p.Brand,
                        Condition = p.Condition,
                        Currency = p.Currency,
                        Description = p.Description,
                        DiscountPrice = p.DiscountPrice,
                        DiscountRate = p.DiscountRate,
                        KDVRate = p.KDVRate,
                        IsActive = p.IsActive,
                        LastPrice = p.LastPrice,
                        MaxOrderQuantity = p.MaxOrderQuantity,
                        MinOrderQuantity = p.MinOrderQuantity,
                        Model = p.Model,
                        Name = p.Name,
                        ProductCode = p.ProductCode,
                        StockQuantity = p.StockQuantity,
                        Tax = p.Tax,
                        UnitPrice = p.UnitPrice,
                        CategoryLists = p.Categorys.Select(p => new View_Models.Category.VM_Result_CategoryList()
                        {
                            CategoryDisplayStatus = p.CategoryDisplayStatus,
                            CategoryLinkTitle = p.CategoryLinkTitle,
                            CategoryName = p.CategoryName,
                            CategorySlug = p.CategorySlug,
                            MaterializedPath = p.MaterializedPath,
                            IsSpecialCategory = p.IsSpecialCategory,
                            IsCampanyCategory = p.IsCampanyCategory,
                            MaterializedPathByName = p.MaterializedPathByName,
                            MaterializedPathBySlug = p.MaterializedPathBySlug,
                        }).ToList()
                    }); 
                    result.Products = productresult;
                    result.ErrorMessage = "";
                    result.HassError = false;
                    result.Message = "İsteğe uygun ürün bulundu";
                    result.StatusCode = System.Net.HttpStatusCode.OK;
                    result.StatusCodeString = System.Net.HttpStatusCode.OK.ToString();
                }


                return result;
            }
            catch(Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.HassError = true;
                result.Message = "(GetSingleByIdQueryHandle) Listeleme sırasında bir hata alındı.";
                result.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                result.StatusCodeString = System.Net.HttpStatusCode.InternalServerError.ToString();
                return result;
            }
        }
    }
}

