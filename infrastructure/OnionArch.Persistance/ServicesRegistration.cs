using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnionArch.Application.Abstractions.AddressServices;
using OnionArch.Application.Abstractions.AttributeServices;
using OnionArch.Application.Abstractions.BasketServices;
using OnionArch.Application.Abstractions.CategoryServices;
using OnionArch.Application.Abstractions.CustomerCrud;
using OnionArch.Application.Abstractions.FileCrud;
using OnionArch.Application.Abstractions.InvoiceFileCrud;
using OnionArch.Application.Abstractions.OrderCrud;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Abstractions.ProductImageFileCrud;
using OnionArch.Application.Abstractions.UserServices;
using OnionArch.Application.Repositories.AddressCrud;
using OnionArch.Application.Repositories.AttributeCrud.AttributeCrud;
using OnionArch.Application.Repositories.AttributeCrud.AttributeValueCrud;
using OnionArch.Application.Repositories.AttributeCrud.CategoryAttributeCrud;
using OnionArch.Application.Repositories.AttributeCrud.ProductAttributeCrud;
using OnionArch.Application.Repositories.BackEndLogsCrud;
using OnionArch.Application.Repositories.BasketCrud;
using OnionArch.Application.Repositories.BasketItemCrud;
using OnionArch.Application.Repositories.BrandAttributeCrud;
using OnionArch.Application.Repositories.BrandCrud;
using OnionArch.Application.Repositories.BrandImageFileCrud;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Application.Repositories.CategoryImageFileCrud;
using OnionArch.Domain.Entities.Identity;
using OnionArch.Persistance.Concretes.CustomerCrud;
using OnionArch.Persistance.Concretes.OrderCrud;
using OnionArch.Persistance.Concretes.ProductCrud;
using OnionArch.Persistance.Contexts;
using OnionArch.Persistance.Repositories.AddressCrud;
using OnionArch.Persistance.Repositories.AttributeCrud.AttributeCrud;
using OnionArch.Persistance.Repositories.AttributeCrud.AttributeValueCrud;
using OnionArch.Persistance.Repositories.AttributeCrud.CategoryAttributeCrud;
using OnionArch.Persistance.Repositories.AttributeCrud.ProductAttributeCrud;
using OnionArch.Persistance.Repositories.BackEndLogsCrud;
using OnionArch.Persistance.Repositories.BasketCrud;
using OnionArch.Persistance.Repositories.BasketItemCrud;
using OnionArch.Persistance.Repositories.BrandAttributeCrud;
using OnionArch.Persistance.Repositories.BrandCrud;
using OnionArch.Persistance.Repositories.BrandImageFileCrud;
using OnionArch.Persistance.Repositories.CategoryCrud;
using OnionArch.Persistance.Repositories.CategoryImageFileCrud;
using OnionArch.Persistance.Repositories.FileCrud;
using OnionArch.Persistance.Repositories.InvoiceFileCrud;
using OnionArch.Persistance.Repositories.OrderCrud;
using OnionArch.Persistance.Repositories.ProductImageFileCrud;
using OnionArch.Persistance.Repositories.UserServices;
using OnionArch.Persistance.ServicesConcreates;
using OnionArch.Application.Repositories.DiscountCouponCrud;
using OnionArch.Application.Abstractions.DiscountServices;
using OnionArch.Persistance.Repositories.PaymentTransactionCrud;
using OnionArch.Application.Repositories.PaymentTransactionCrud;
using OnionArch.Application.Abstractions.OrderServices;
using OnionArch.Application.Abstractions.PaymentServices;
using OnionArch.Persistance.Repositories.DiscountCouponCrud;
using OnionArch.Application.Repositories.HeroSectionCruds.HeroSectionImageCrud;
using OnionArch.Application.Repositories.HeroSectionCruds.HeroSectionSliderCrud;
using OnionArch.Persistance.Repositories.HeroSectionCruds.HeroSectionSliderCrud;

namespace OnionArch.Persistance
{
    public static class ServicesRegistration
	{
		public static void AddPersistanceServices(this IServiceCollection services)
        {

            services.AddDbContext<OnionArchDBContext>(options => options.UseNpgsql(Configuration.ConnectionString));

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

            services.AddScoped<ICategoryImageFileReadRepository, CategoryImageFileReadRepository>();
            services.AddScoped<ICategoryImageFileWriteRepository, CategoryImageFileWriteRepository>();


            services.AddScoped<IBackEndLogsReadRepository, BackEndLogsReadRepository>();
            services.AddScoped<IBackEndLogsWriteRepository, BackEndLogsWriteRepository>();


            services.AddScoped<IBasketReadRepository, BasketReadRepository>();
            services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();

            services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
            services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();



            services.AddScoped<ICategoryReadRepository , CategoryReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();

            services.AddScoped<IUserService,UserService>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IBasketService, BasketService>();

            services.AddScoped<IAddressWriteRepository, AddressWriteRepository>();
            services.AddScoped<IAddressReadRepository, AddressReadRepository>();
            services.AddScoped<IAddressService,AddressService>();

            services.AddScoped<ICategoryServices, CategoryServices>();


            services.AddScoped<IAttributeReadRepository, AttributeReadRepository>();
            services.AddScoped<IAttributeWriteRepository, AttributeWriteRepository>();

            services.AddScoped<IAttributeValueReadRepository, AttributeValueReadRepository>();
            services.AddScoped<IAttributeValueWriteRepository, AttributeValueWriteRepository>();

            services.AddScoped<ICategoryAttributeReadRepository, CategoryAttributeReadRepository>();
            services.AddScoped<ICategoryAttributeWriteRepository, CategoryAttributeWriteRepository>();

            services.AddScoped<IProductAttributeReadRepository, ProductAttributeReadRepository>();
            services.AddScoped<IProductAttributeWriteRepository, ProductAttributeWriteRepository>();
            
            services.AddScoped<IAttributeService, AttributeService>();


            services.AddScoped<IBrandReadRepository, BrandReadRepository>();
            services.AddScoped<IBrandWriteRepository, BrandWriteRepository>();

            services.AddScoped<IBrandImageFileReadRepository, BrandImageFileReadRepository>();
            services.AddScoped<IBrandImageFileWriteRepository, BrandImageFileWriteRepository>();

            services.AddScoped<IBrandAttributeReadRepository, BrandAttributeReadRepository>();
            services.AddScoped<IBrandAttributeWriteRepository, BrandAttributeWriteRepository>();

            services.AddScoped<IDiscountCouponReadRepository, DiscountCouponReadRepository>();
            services.AddScoped<IDiscountCouponWriteRepository, DiscountCouponWriteRepository>();
            services.AddScoped<IDiscountCouponService, DiscountCouponService>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentTransactionWriteRepository, PaymentTransactionWriteRepository>();
            services.AddScoped<IPaymentTransactionReadRepository, PaymentTransactionReadRepository>();
            services.AddScoped<IPaymentService, PaymentService>();



            services.AddScoped<IHeroSectionImageReadRepository, HeroSectionImageReadRepository>();
            services.AddScoped<IHeroSectionImageWriteRepository, HeroSectionImageWriteRepository>();

            services.AddScoped<IHeroSectionSliderReadRepository, HeroSectionSliderReadRepository>();
            services.AddScoped<IHeroSectionSliderWriteRepository, HeroSectionSliderWriteRepository>();

        }
	}
}

