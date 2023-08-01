using System;
using OnionArch.Application.Abstractions.CustomerCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using OnionArch.Persistance.Repositorys;

namespace OnionArch.Persistance.Concretes.CustomerCrud
{
    public class CustomerWriteRepository : WriteRepository<Customer>, ICustomerWriteRepository
    {
        //Şimdi burada sadece ICustomerWriteRepository uygularsak bütün içerikleri implement eder ve tekrardan oluşturmamız gerekir,
        //ama biz burada oluşturduğumuz WriteRepository içinde bunu zaten yaptın buna gerek yok  o zaman bunu şöyle yapamazmıyız
        //Tüm bu düzenlemeleri sen git WriteRepository içinde yap.
        //Aynı zamanda  ICustomerWriteRepository senin soyut nesnen olsun.

        public CustomerWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}

