using System;
using OnionArch.Application.Abstractions.CustomerCrud;
using OnionArch.Application.Abstractions.OrderCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using OnionArch.Persistance.Repositorys;

namespace OnionArch.Persistance.Concretes.OrderCrud
{
    public class OrderReadRepository : ReadRepository<Order>, IOrderReadRepository
    {

        //Şimdi burada sadece ICustomerReadRepository uygularsak bütün içerikleri implement eder ve tekrardan oluşturmamız gerekir,
        //ama biz burada oluşturduğumuz ReadRepositoy içinde bunu zaten yaptın buna gerek yok  o zaman bunu şöyle yapamazmıyız
        //Tüm bu düzenlemeleri sen git readRepository içinde yap.
        //Aynı zamanda  ICustomerReadRepository senin soyut nesnen olsun.


        public OrderReadRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}

