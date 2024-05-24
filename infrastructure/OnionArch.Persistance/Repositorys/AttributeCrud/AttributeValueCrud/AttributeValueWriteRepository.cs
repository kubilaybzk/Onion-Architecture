using OnionArch.Application.Repositories.AttributeCrud.AttributeValueCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Repositorys.AttributeCrud.AttributeValueCrud
{
    public class AttributeValueWriteRepository : WriteRepository<AttributeValue>, IAttributeValueWriteRepository
    {
        public AttributeValueWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
