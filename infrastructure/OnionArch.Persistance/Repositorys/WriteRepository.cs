using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OnionArch.Application.Abstractions;
using OnionArch.Domain.Entities.Common;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance.Repositorys
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {


        //Şimdi burada bu işlemleri yapabilmemiz için ilk olarak bizim DBContext nesnemize ihtiyacımız var.

        private readonly OnionArchDBContext _context;

        //Kullanmak için inject etmemiz gerekiyor.
        public WriteRepository(OnionArchDBContext context)
        {
            _context = context;
        }

        //Burada EFCore'da set<t> gerekli olan nesneyi Dbset olarak döndürüyor burada buna neden ihtiyacımız var
        //normal şartlarda _context.Set<T>.ToList gibi fonksiyonlarda   _context.Set<T> yazmamızı kısaltacağız.





        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AddAsync(T model)
        {
          EntityEntry<T> result=  await Table.AddAsync(model);

            //State değerine bakıyoruz eğer ekleme ise ekle diyoruz değilse false dönüyor.
            return result.State == EntityState.Added;

        }

        public async Task<bool> AddRangeAsync(List<T> datas)
        {
            await Table.AddRangeAsync(datas);
            return true;
        }



        public bool Remove(T datas)
        {
            EntityEntry<T> result = Table.Remove(datas);
            return result.State == EntityState.Deleted;
        }

        bool IWriteRepository<T>.RemoveRange(List<T> datas)
        {
            Table.RemoveRange(datas);
            return true;
        }

        public async Task<bool> RemoveAsync(string id)
        {
            T result =  await Table.FirstOrDefaultAsync(p => p.ID == Guid.Parse(id));
            return Remove(result);

        }



        public bool Update(T model)
        {
            EntityEntry<T> result =  Table.Update(model);
            return result.State == EntityState.Modified;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

