using System;
using OnionArch.Application.Abstractions.OrderCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.Repositories.OrderCrud
{
    public class OrderWriteRepository : WriteRepository<Order>, IOrderWriteRepository
    {
        public OrderWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}

