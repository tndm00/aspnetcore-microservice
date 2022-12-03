using FluentValidation;
using System.Reflection;
using MediatR;
using Ordering.Application.Common.Behaviours;

namespace Ordering.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
            services.AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddMediatR(Assembly.GetExecutingAssembly())
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandleExceptionBehiviour<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }
}
