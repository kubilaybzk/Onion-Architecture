using System;
using Microsoft.EntityFrameworkCore;
using OnionArch.Domain.Entities;

namespace OnionArch.Persistance.Contexts
{
	public class OnionArchDBContext:DbContext
	{
        //Burada DBContext'i IOC container içinde tanımlayacağız. ServicesRegistration içinde bu işlemi gerçekleştireceğiz.
        public OnionArchDBContext(DbContextOptions options) : base(options) { }

		public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }




    }
}

