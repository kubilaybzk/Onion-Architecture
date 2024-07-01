using System;
using MediatR;
using OnionArch.Application.Abstractions.ProductCrud;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.View_Models.Product;
using OnionArch.Application.View_Models.Category;

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
                var product =  _productReadRepository.GetWhere(p => p.ID == Guid.Parse(request.id.ToString()))
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
                        AppliedDiscountRate = p.DiscountRate,
                        Brand = p.Brand,
                        CategoryLists = p.Categorys.Select(p=>new VM_Result_CategoryList()
                        {
                            MaterializedPathByName = p.MaterializedPathByName,
                            MaterializedPathBySlug = p.MaterializedPathBySlug,
                            Id=p.ID,
                            CategoryName= p.CategoryName,
                        }).ToList(),
                        Condition = p.Condition,
                        Currency = p.Currency,
                        SmallDescription = p.SmallDescription,
                        LongDescription = p.LongDescription,
                        DiscountPrice = p.DiscountPrice,
                        DiscountRate = p.DiscountRate,
                        Id = Guid.Parse(p.ID.ToString()),
                        IsActive = p.IsActive,
                        KDVRate = p.KDVRate,
                        LastPrice = p.LastPrice,
                        MaxOrderQuantity = p.MaxOrderQuantity,
                        MinOrderQuantity = p.MinOrderQuantity,
                        Model = p.Model,
                        Name = p.Name,
                        ProductAttributes = null,
                        ProductCode = p.ProductCode,
                        ProductImageFiles = p.ProductImageFiles.Select(p => new Domain.Entities.ProductImageFile()
                        {
                            FileName = p.FileName,
                            CreateTime = DateTime.Now,
                            ID = p.ID,
                            Path = p.Path,
                            Showcase = p.Showcase,
                            Storage = p.Storage,
                            UpdateTime = DateTime.Now,
                        }).ToList(),
                        StockQuantity = p.StockQuantity,
                        Tax = p.Tax,
                        UnitPrice = p.UnitPrice,
                        MaterializedProductPath = p.MaterializedProductPath,
                        MaterializedProductPathByName = p.MaterializedProductPathByName,
                        MaterializedProductPathBySlug = p.MaterializedProductPathBySlug
                    }).ToList(); 
                   
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

