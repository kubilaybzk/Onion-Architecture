using OnionArch.Application.Repositories.BrandAttributeCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Repositorys.BrandAttributeCrud
{
    public class BrandAttributeWriteRepository : WriteRepository<BrandAttribute>, IBrandAttributeWriteRepository
    {
        public BrandAttributeWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
