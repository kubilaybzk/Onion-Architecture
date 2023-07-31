using System;
using Microsoft.Extensions.DependencyInjection;
using OnionArch.Application.Abstractions;
using OnionArch.Persistance.Concretes;

namespace OnionArch.Persistance
{
	public static class ServicesRegistration
	{
		public static void AddPersistanceServices(this IServiceCollection service)
			=>service.AddSingleton<IProductServices, ProductServices>();

        
	}
}

