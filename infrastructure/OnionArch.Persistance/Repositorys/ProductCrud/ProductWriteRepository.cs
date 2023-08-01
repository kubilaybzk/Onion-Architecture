using System;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using OnionArch.Persistance.Repositorys;

namespace OnionArch.Persistance.Concretes.ProductCrud
{
    public class ProductWriteRepository : WriteRepository<Product>, IProductWriteRepository
    {
        public ProductWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}

