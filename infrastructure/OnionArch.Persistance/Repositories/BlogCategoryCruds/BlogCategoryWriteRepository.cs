using OnionArch.Application.Repositories.BlogCategoryCruds;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.Repositories.BlogCategoryCruds
{
    public class BlogCategoryWriteRepository : WriteRepository<BlogCategory>, IBlogCategoryWriteRepository
    {
        public BlogCategoryWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}