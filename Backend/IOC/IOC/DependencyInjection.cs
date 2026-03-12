using Application;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace IOC;

public static class DependencyInjection
{
    public static IServiceCollection AddAllServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddPersistence(configuration)
            .AddInfrastructure()
            .AddApplication();

        return services;
    }
}