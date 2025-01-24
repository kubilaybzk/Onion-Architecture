using System;
using OnionArch.Application.Abstractions.FileCrud;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.Repositories.FileCrud
{
    public class FileReadRepository : ReadRepository<OnionArch.Domain.Entities.File>, IFileReadRepository
    {
        public FileReadRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}

