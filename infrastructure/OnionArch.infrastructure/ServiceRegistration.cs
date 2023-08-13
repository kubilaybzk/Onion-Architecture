using System;
using Microsoft.Extensions.DependencyInjection;
using OnionArch.Application.Services;
using OnionArch.infrastructure.Services;

namespace OnionArch.infrastructure
{
	public static class ServiceRegistration
	{
        public static void AddInfrastructureServices(this IServiceCollection services)
        {

            services.AddScoped<IFileService, FileService>();

        }
    }
}

