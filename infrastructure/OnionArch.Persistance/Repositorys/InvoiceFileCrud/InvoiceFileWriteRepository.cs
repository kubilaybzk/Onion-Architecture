using System;
using OnionArch.Application.Abstractions.InvoiceFileCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.Repositorys.InvoiceFileCrud
{
    public class InvoiceFileWriteRepository : WriteRepository<InvoiceFile>, IInvoiceFileWriteRepository
    {
        public InvoiceFileWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}

