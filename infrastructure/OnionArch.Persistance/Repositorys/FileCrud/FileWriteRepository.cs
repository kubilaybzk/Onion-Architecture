using System;
using OnionArch.Application.Abstractions.FileCrud;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.Repositorys.FileCrud
{
    public class FileWriteRepository : WriteRepository<OnionArch.Domain.Entities.File>, IFileWriteRepository
    {
        public FileWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}

