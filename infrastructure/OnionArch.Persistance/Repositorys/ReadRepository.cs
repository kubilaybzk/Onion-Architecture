using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions;
using OnionArch.Domain.Entities.Common;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.Repositorys
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {

        //Şimdi burada bu işlemleri yapabilmemiz için ilk olarak bizim DBContext nesnemize ihtiyacımız var.

        private readonly OnionArchDBContext _context;

        //Kullanmak için inject etmemiz gerekiyor.
        public ReadRepository(OnionArchDBContext context)
        {
            _context = context;
        }

        //Burada EFCore'da set<t> gerekli olan nesneyi Dbset olarak döndürüyor burada buna neden ihtiyacımız var
        //normal şartlarda _context.Set<T>.ToList gibi fonksiyonlarda   _context.Set<T> yazmamızı kısaltacağız.



        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }
        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method);
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = Table.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        }
        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        //=> await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
        //=> await Table.FindAsync(Guid.Parse(id));
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = Table.AsNoTracking();
            return await query.FirstOrDefaultAsync(data => data.ID == Guid.Parse(id));
        }

    }

}