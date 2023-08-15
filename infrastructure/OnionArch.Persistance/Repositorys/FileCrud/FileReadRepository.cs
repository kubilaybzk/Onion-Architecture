using System;
using OnionArch.Application.Abstractions.FileCrud;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.Repositorys.FileCrud
{
    public class FileReadRepository : ReadRepository<OnionArch.Domain.Entities.File>, IFileReadRepository
    {
        public FileReadRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}

