using OnionArch.Application.Abstractions;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.Repositories.CategoryCrud
{
    public class CategoryWriteRepository : WriteRepository<Category>, ICategoryWriteRepository
    {
        public CategoryWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}