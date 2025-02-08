using OnionArch.Application.Repositories.BlogCategoryCruds;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.Repositories.BlogCategoryCruds
{
    public class BlogCategoryReadRepository : ReadRepository<BlogCategory>, IBlogCategoryReadRepository
    {
        public BlogCategoryReadRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}