using OnionArch.Application.Abstractions;
using OnionArch.Application.Repositories.AttributeCrud.CategoryAttributeCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Repositorys.AttributeCrud.CategoryAttributeCrud
{
    public class CategoryAttributeWriteRepository : WriteRepository<CategoryAttribute>, ICategoryAttributeWriteRepository
    {
        public CategoryAttributeWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
