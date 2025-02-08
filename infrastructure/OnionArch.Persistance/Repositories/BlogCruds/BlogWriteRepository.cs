using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Repositories.BlogCruds;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.Repositories.BlogCruds
{
    public class BlogWriteRepository : WriteRepository<Blog>, IBlogWriteRepository
    {
        private readonly OnionArchDBContext _context;

        public BlogWriteRepository(OnionArchDBContext context) : base(context)
        {
            _context = context;
        }

 
    }
}