using Alti.Domain.Factories.Implementations;
using Alti.Domain.Factories.Interfaces;
using Alti.Domain.Services.Implementations;
using Alti.Domain.Services.Interfaces;
using Application.Interfaces;
using Infrastructure.Logging.Implementations;
using Infrastructure.Logging.Interfaces;
using Infrastructure.Security.Implementations;
using Infrastructure.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Domain Factories
        services.AddScoped<IUserFactory, UserFactory>();
        services.AddScoped<IRoomFactory, RoomFactory>();
        services.AddScoped<ISeasonFactory, SeasonFactory>();
        services.AddScoped<IRateFactory, RateFactory>();
        services.AddScoped<IBookingFactory, BookingFactory>();
        services.AddScoped<IPaymentFactory, PaymentFactory>();
        services.AddScoped<IAdditionalServiceFactory, AdditionalServiceFactory>();
        services.AddScoped<IBookingServiceFactory, BookingServiceFactory>();
        services.AddScoped<IAuditLogFactory, AuditLogFactory>();

        // Domain Services
        services.AddScoped<IUserDomainService, UserDomainService>();
        services.AddScoped<IRoomDomainService, RoomDomainService>();
        services.AddScoped<IBookingDomainService, BookingDomainService>();
        services.AddScoped<IPaymentDomainService, PaymentDomainService>();
        services.AddScoped<ISeasonDomainService, SeasonDomainService>();
        services.AddScoped<IRateDomainService, RateDomainService>();
        services.AddScoped<IAdditionalServiceDomainService, AdditionalServiceDomainService>();

        // Infrastructure Services
        services.AddScoped<ILoggerService, LoggerService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ICodeGeneratorService, CodeGeneratorService>();

        return services;
    }
}