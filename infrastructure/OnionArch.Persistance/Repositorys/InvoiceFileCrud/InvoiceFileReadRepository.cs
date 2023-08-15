using System;
using OnionArch.Application.Abstractions.InvoiceFileCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.Repositorys.InvoiceFileCrud
{
    public class InvoiceFileReadRepository : ReadRepository<InvoiceFile>, IInvoiceFileReadRepository
    {
        public InvoiceFileReadRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}

