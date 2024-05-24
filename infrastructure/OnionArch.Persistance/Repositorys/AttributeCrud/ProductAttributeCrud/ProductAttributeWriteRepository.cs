using OnionArch.Application.Repositories.AttributeCrud.ProductAttributeCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Repositorys.AttributeCrud.ProductAttributeCrud
{
    public class ProductAttributeWriteRepository : WriteRepository<ProductAttribute>, IProductAttributeWriteRepository
    {
        public ProductAttributeWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
