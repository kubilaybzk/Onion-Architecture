using OnionArch.Application.Repositories.BrandCrud;
using OnionArch.Application.Repositories.BrandImageFileCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Repositorys.BrandImageFileCrud
{
    public class BrandImageFileReadRepository : ReadRepository<BrandImageFile>, IBrandImageFileReadRepository
    {
        public BrandImageFileReadRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
