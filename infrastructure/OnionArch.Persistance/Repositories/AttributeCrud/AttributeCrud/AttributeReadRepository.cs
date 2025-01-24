using OnionArch.Application.Repositories.AttributeCrud.AttributeCrud;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Repositories.AttributeCrud.AttributeCrud
{
    public class AttributeReadRepository : ReadRepository<Domain.Entities.Attribute>, IAttributeReadRepository
    {
        public AttributeReadRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
