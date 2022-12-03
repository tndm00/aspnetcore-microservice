using Microsoft.EntityFrameworkCore;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"),
                    builder => builder.MigrationsAssembly(typeof(OrderContext).Assembly.FullName));
            });

            services.AddScoped<OrderContextSeed>();

            return services;
        }
    }
}
