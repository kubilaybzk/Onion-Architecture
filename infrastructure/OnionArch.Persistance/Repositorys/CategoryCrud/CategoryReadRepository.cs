using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Repositorys.CategoryCrud
{
    public class CategoryReadRepository : ReadRepository<Category>, ICategoryReadRepository
    {
        public CategoryReadRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
