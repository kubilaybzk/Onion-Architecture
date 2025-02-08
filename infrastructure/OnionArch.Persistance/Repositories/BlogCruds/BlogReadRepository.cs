using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Repositories.BlogCruds;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.Repositories.BlogCruds
{
    public class BlogReadRepository : ReadRepository<Blog>, IBlogReadRepository
    {
        private readonly OnionArchDBContext _context;

        public BlogReadRepository(OnionArchDBContext context) : base(context)
        {
            _context = context;
        }

        
    }
}