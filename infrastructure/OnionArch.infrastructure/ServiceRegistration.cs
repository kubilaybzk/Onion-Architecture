using System;
using Microsoft.Extensions.DependencyInjection;
using OnionArch.Application.Abstractions.Storage;
using OnionArch.Application.Abstractions.Token;
using OnionArch.infrastructure.Services;
using OnionArch.infrastructure.Services.Storage;
using OnionArch.infrastructure.Services.Token;

namespace OnionArch.infrastructure
{
	public static class ServiceRegistration
	{
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            //Burada normal şekilde bir servis kaydını oluşturuyoruz.
            //Bu sayede  IStorageService çağrıldığı zaman bize  StorageService oluşturuyor.
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<ITokenHandler, TokenHandler>();
        }

        //Şimdi bahsettiğimiz gibi program.çs üzerinden hangi Storage yönteminin çalışacağını ayarlayalım.


        public static void AddStorage<T>(this IServiceCollection services) where T :  class , IStorage
        {
            //Burada diyoruz ki sana bir IStorage yapısı gelecek bu gelen yapı IStorage'dan türemiş olacak
            //Sen buna göre memory üzerinde bir referans oluştur.
            services.AddScoped<IStorage,T>();
        }
    }
}

