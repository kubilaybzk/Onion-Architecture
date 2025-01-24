using OnionArch.Application.Repositories.BrandAttributeCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Repositories.BrandAttributeCrud
{
    public class BrandAttributeReadRepository : ReadRepository<BrandAttribute>, IBrandAttributeReadRepository
    {
        public BrandAttributeReadRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
