using OnionArch.Application.Repositories.AttributeCrud.AttributeValueCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Repositories.AttributeCrud.AttributeValueCrud
{
    public class AttributeValueReadRepository : ReadRepository<AttributeValue>, IAttributeValueReadRepository
    {
        public AttributeValueReadRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
