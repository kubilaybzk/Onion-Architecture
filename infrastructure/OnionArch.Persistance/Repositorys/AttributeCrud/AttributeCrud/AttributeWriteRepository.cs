using OnionArch.Application.Repositories.AttributeCrud.AttributeCrud;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Repositorys.AttributeCrud.AttributeCrud
{
    public class AttributeWriteRepository : WriteRepository<Domain.Entities.Attribute>, IAttributeWriteRepository
    {
        public AttributeWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
