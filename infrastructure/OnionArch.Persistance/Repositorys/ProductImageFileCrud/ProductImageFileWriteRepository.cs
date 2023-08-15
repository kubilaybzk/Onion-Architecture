using System;
using OnionArch.Application.Abstractions.ProductImageFileCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.Repositorys.ProductImageFileCrud
{
    public class ProductImageFileWriteRepository : WriteRepository<ProductImageFile>, IProductImageFileWriteRepository
    {
        public ProductImageFileWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}

