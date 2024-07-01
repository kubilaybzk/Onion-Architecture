using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.View_Models.Category;
using OnionArch.Application.View_Models.Product;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.Product.GetLatesProducts
{
    public class GetLatesProductsHandler : IRequestHandler<GetLatesProductsRequest, GetLatesProductsResponse>
    {
        private readonly IProductReadRepository _productReadRepository;

        public GetLatesProductsHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<GetLatesProductsResponse> Handle(GetLatesProductsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _productReadRepository.Table
                    .Include(p => p.ProductImageFiles)
                    .Include(p => p.Categorys)
                        .Where(p => string.IsNullOrEmpty(request.ProductCodeOrProductName) ||
                        p.Name.Contains(request.ProductCodeOrProductName) ||
                        p.ProductCode.Contains(request.ProductCodeOrProductName))
                    .AsQueryable();

                

                var totalCount = await query.CountAsync();
                var totalPageSize = (int)Math.Ceiling(totalCount / (double)request.Size);

                var products = await query
                    .OrderByDescending(p => p.CreateTime)
                    .Skip((request.Page - 1) * request.Size)
                    .Take(request.Size)
                    .Select(p => new VM_Result_ProductLink
                    {
                        AppliedDiscountRate = p.AppliedDiscountRate,
                        AppliedDiscountPrice = p.AppliedDiscountPrice,
                        Brand = p.Brand,
                        Condition = p.Condition,
                        Currency = p.Currency,
                        SmallDescription = p.SmallDescription,
                        LongDescription = p.LongDescription,
                        DiscountPrice = p.DiscountPrice,
                        DiscountRate = p.DiscountRate,
                        Id = p.ID,
                        IsActive = p.IsActive,
                        KDVRate = p.KDVRate,
                        LastPrice = p.LastPrice,
                        MaxOrderQuantity = p.MaxOrderQuantity,
                        MinOrderQuantity = p.MinOrderQuantity,
                        Model = p.Model,
                        Name = p.Name,
                        ProductCode = p.ProductCode,
                        ProductImageFiles = p.ProductImageFiles.Select(img => new Domain.Entities.ProductImageFile
                        {
                            FileName = img.FileName,
                            CreateTime = img.CreateTime,
                            ID = img.ID,
                            Path = img.Path,
                            Showcase = img.Showcase,
                            Storage = img.Storage,
                            UpdateTime = img.UpdateTime,
                        }).ToList(),
                        StockQuantity = p.StockQuantity,
                        Tax = p.Tax,
                        UnitPrice = p.UnitPrice,
                        CategoryLists = p.Categorys.Select(p => new VM_Result_CategoryList() { CategoryName = p.CategoryName, }).ToList(),
                        MaterializedProductPath = p.MaterializedProductPath,
                        MaterializedProductPathByName = p.MaterializedProductPathByName,
                        MaterializedProductPathBySlug = p.MaterializedProductPathBySlug
                    })
                   .ToListAsync();

                bool hasNextPage = request.Page < totalPageSize;
                bool hasPrevPage = request.Page > 1;

                return new GetLatesProductsResponse()
                {
                    ErrorMessage = "",
                    HassError = false,
                    Message = "Ürünler başarıyla listelendi",
                    Products = products,
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString(),
                    TotalCount = totalCount,
                    TotalPageSize = totalPageSize,
                    CurrentPage = request.Page,
                    HasNext = hasNextPage,
                    HasPrev = hasPrevPage,
                    PageSize = request.Size,
                };
            }
            catch (Exception ex)
            {
                return new GetLatesProductsResponse()
                {
                    ErrorMessage = ex.Message,
                    HassError = true,
                    Message = "Ürünler listelenirken hata alındı",
                    Products = null,
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString(),
                };
            }
        }
    }
}
