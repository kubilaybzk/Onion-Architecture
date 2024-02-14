using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnionArch.Domain.Entities;
using OnionArch.Domain.Entities.Common;
using OnionArch.Domain.Entities.Identity;

namespace OnionArch.Persistance.Contexts
{
	public class OnionArchDBContext:IdentityDbContext<AppUser,AppRole,string>
	{
        //Burada DBContext'i IOC container içinde tanımlayacağız. ServicesRegistration içinde bu işlemi gerçekleştireceğiz.
        public OnionArchDBContext(DbContextOptions options) : base(options) { }

		public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        //Burada Bizim File Entity'miz .Net'in File classı ile karışıyor bunu engellemek için böyle bir yöntem kullandık.
        public DbSet<Domain.Entities.File> Files { get; set; }

        public DbSet<ProductImageFile> ProductImageFiles { get; set; }

        public DbSet<InvoiceFile> InvoiceFiles { get; set; }

        public DbSet<BackEndLogs> BackEndLogs { get; set; }

        public DbSet<Basket> Baskets{ get; set; }

        public DbSet<BasketItem> BasketItems { get; set; }

        public DbSet<Address> Addresses { get; set; }

        //Burada veri tabanında otomatik olarka yapılan işlemlerde EFCore tarafından belirli alanlara değerler atanmasını istiyoruz.
        //Base entity içinde bulunana update ve createTime alanlarının
        ///her bir savechanges anında değişmesini ve bu değerin otomatik olarak EFCore tarafından tanımkanmasını istiyoruz


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Order>()
                .HasKey(b => b.ID);

            builder.Entity<Basket>()
                .HasOne(b => b.Order)
                .WithOne(b => b.Basket)
                .HasForeignKey<Order>(b => b.BasketId);


            base.OnModelCreating(builder); // Biz IdentityDbContext kullandığımız için bunu eklemek zorundayız.


        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();
            //ChangeTracker tarafından yakalanan her bir veriyi ForEach ile beraber dönüyoruz.
            foreach (var data in datas)
            {
                switch (data.State)
                {
                    //Eklenme işlemi varsa CreateTime değerini atıyoruz.
                    case EntityState.Added:
                        data.Entity.CreateTime = DateTime.Now;
                        break;
                    //Değişme işlemi varsa CreateTime değerini atıyoruz.
                    case EntityState.Modified:
                        data.Entity.UpdateTime = DateTime.Now;
                        break;
                    //Silme işlemi varsa Burada hiçbir şey yapmasını istiyoruz Burada EFCore bu Case olmazsa hataya düşüyor.
                    case EntityState.Deleted:
                        break;
                    default:
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }




    }
}

