using Alti.Core.Domain.Interfaces;
using Alti.Infrastructure.Repositories.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Alti.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services;
    }
}