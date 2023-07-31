using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OnionArch.Application.Abstractions;
using OnionArch.Persistance.Concretes;
using OnionArch.Persistance.Contexts;

namespace OnionArch.Persistance
{
	public static class ServicesRegistration
	{
		public static void AddPersistanceServices(this IServiceCollection service)
			=> service.AddDbContext<OnionArchDBContext>(options=> options.UseSqlServer(Configuration.ConnectionString));
	}
}

