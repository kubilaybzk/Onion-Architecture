using System;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using OnionArch.Persistance.Repositorys;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OnionArch.Persistance.Concretes.ProductCrud
{
    public class ProductReadRepository : ReadRepository<Product>, IProductReadRepository
    {

        //Şimdi burada sadece ICustomerReadRepository uygularsak bütün içerikleri implement eder ve tekrardan oluşturmamız gerekir,
        //ama biz burada oluşturduğumuz ReadRepositoy içinde bunu zaten yaptın buna gerek yok  o zaman bunu şöyle yapamazmıyız
        //Tüm bu düzenlemeleri sen git readRepository içinde yap.
        //Aynı zamanda  ICustomerReadRepository senin soyut nesnen olsun.


        public ProductReadRepository(OnionArchDBContext context) : base(context)
        {

        }

        //GetAll methodunu burada override ediyoruz.

        public override IQueryable<Product> GetAll(bool tracking = true)
        {
            //Base'den Table değerimize ulaşıyoruz .
            //Daha sonra  Include sayesibde bağlantıyı kuruyoruz.
            var query = Table.Include(p => p.ProductImageFiles).AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public async override Task<Product> GetByIdAsync(string id, bool tracking = true)
        {
            var query = Table.Include(p => p.ProductImageFiles).AsQueryable();
            if (!tracking)
                query = Table.AsNoTracking();
            return await query.FirstOrDefaultAsync(data => data.ID == Guid.Parse(id));

        }




    }
}

