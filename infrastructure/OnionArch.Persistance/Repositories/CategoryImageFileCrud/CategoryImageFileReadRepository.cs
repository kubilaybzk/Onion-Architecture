using OnionArch.Application.Repositories.CategoryImageFileCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Repositories.CategoryImageFileCrud
{
    public class CategoryImageFileReadRepository : ReadRepository<CategoryImageFile>, ICategoryImageFileReadRepository
    {
        public CategoryImageFileReadRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
