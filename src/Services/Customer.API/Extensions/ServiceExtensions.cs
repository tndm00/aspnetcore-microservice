﻿using Customer.API.Porsistence;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services;
using Customer.API.Services.Interfaces;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.Configurations;

namespace Customer.API.Extensions
{
    public static class ServiceExtensions
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = configuration.GetSection(nameof(DatabaseSettings))
                .Get<DatabaseSettings>();

            services.AddSingleton(databaseSettings);

            var hangfireSettings = configuration.GetSection(nameof(HangFireSettings))
                .Get<HangFireSettings>();

            services.AddSingleton(hangfireSettings);
            return services;
        }

        public static void ConfigureCustomerContext(this IServiceCollection services)
        {
            var databaseSettings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
            if (databaseSettings == null || string.IsNullOrEmpty(databaseSettings.ConnectionString))
                throw new ArgumentException("Connection string is not configuired");

            services.AddDbContext<CustomerContext>(options => options.UseNpgsql(databaseSettings.ConnectionString));
        }

        public static void AddInfastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>()
                .AddScoped<ICustomerService, CustomerService>();
        }
    }
}
