using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OnionArch.Application.Abstractions;
using OnionArch.Application.Abstractions.CustomerCrud;
using OnionArch.Application.Abstractions.OrderCrud;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Persistance.Concretes;
using OnionArch.Persistance.Concretes.CustomerCrud;
using OnionArch.Persistance.Concretes.OrderCrud;
using OnionArch.Persistance.Concretes.ProductCrud;
using OnionArch.Persistance.Contexts;
using OnionArch.Persistance.Repositorys.OrderCrud;

namespace OnionArch.Persistance
{
	public static class ServicesRegistration
	{
		public static void AddPersistanceServices(this IServiceCollection services)
        {

            services.AddDbContext<OnionArchDBContext>(options => options.UseSqlServer(Configuration.ConnectionString));


            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();

        }
	}
}

