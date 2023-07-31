using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<OnionArchDBContext>
    {
        public OnionArchDBContext CreateDbContext(string[] args)
        {

            DbContextOptionsBuilder<OnionArchDBContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseSqlServer(Configuration.ConnectionString);
            return new(dbContextOptionsBuilder.Options);
        }
    }

}

