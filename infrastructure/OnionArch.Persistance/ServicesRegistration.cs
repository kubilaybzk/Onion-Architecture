using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnionArch.Application.Abstractions.CustomerCrud;
using OnionArch.Application.Abstractions.FileCrud;
using OnionArch.Application.Abstractions.InvoiceFileCrud;
using OnionArch.Application.Abstractions.OrderCrud;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Abstractions.ProductImageFileCrud;
using OnionArch.Application.Abstractions.UserServices;
using OnionArch.Domain.Entities.Identity;
using OnionArch.Persistance.Concretes.CustomerCrud;
using OnionArch.Persistance.Concretes.OrderCrud;
using OnionArch.Persistance.Concretes.ProductCrud;
using OnionArch.Persistance.Contexts;
using OnionArch.Persistance.Repositorys.FileCrud;
using OnionArch.Persistance.Repositorys.InvoiceFileCrud;
using OnionArch.Persistance.Repositorys.OrderCrud;
using OnionArch.Persistance.Repositorys.ProductImageFileCrud;
using OnionArch.Persistance.Repositorys.UserServices;

namespace OnionArch.Persistance
{
    public static class ServicesRegistration
	{
		public static void AddPersistanceServices(this IServiceCollection services)
        {

            services.AddDbContext<OnionArchDBContext>(options => options.UseSqlServer(Configuration.ConnectionString));

            //Identity için gerekli olan düzenlemeler . 
            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<OnionArchDBContext>();


            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();


            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();



            services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
            services.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>();



            services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
            services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();

            services.AddScoped<IUserService,UserService>();

            services.AddScoped<IAuthService, AuthService>();




        }
	}
}

