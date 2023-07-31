using System;
using OnionArch.Application.Abstractions;
using OnionArch.Domain.Entities;

namespace OnionArch.Persistance.Concretes
{
    public class ProductServices : IProductServices
    {



        public List<Product> GetProducts()
        {
            return (
                new List<Product>
                {
                    new Product { ID = Guid.NewGuid(),Stock=21, Price = 2400, Name = "Product1" },
                     new Product { ID = Guid.NewGuid(),Stock=22, Price = 2401, Name = "Product2" },
                      new Product { ID = Guid.NewGuid(),Stock=23, Price = 2402, Name = "Product3" },
                       new Product { ID = Guid.NewGuid(),Stock=24, Price = 2403, Name = "Product4" },
                        new Product { ID = Guid.NewGuid(),Stock=25, Price = 2404, Name = "Product5" },
                }
                );
        }

        
    }
}

