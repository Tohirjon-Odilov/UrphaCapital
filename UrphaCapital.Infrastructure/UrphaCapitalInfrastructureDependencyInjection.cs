﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrphaCapital.Application.Abstractions;
using UrphaCapital.Infrastructure.Persistanse;

namespace UrphaCapital.Infrastructure
{
    public static class UrphaCapitalInfrastructureDependencyInjection
    {
        public static IServiceCollection AddUrphaCapitalInfrastructureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IApplicationDbContext, UrphaCapitalDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Db"));
            });

            return services;
        }
    }
}
