using System;
using OnionArch.Application.Abstractions.ProductImageFileCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.Repositorys.ProductImageFileCrud
{
    public class ProductImageFileReadRepository : ReadRepository<ProductImageFile>, IProductImageFileReadRepository
    {
        public ProductImageFileReadRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}

