using Alti.Domain.Factories.Implementations;
using Alti.Domain.Factories.Interfaces;
using Alti.Domain.Interfaces;
using Alti.Domain.Services.Implementations;
using Alti.Domain.Services.Interfaces;
using Infrastructure.Logging.Implementations;
using Infrastructure.Logging.Interfaces;
using Infrastructure.Security.Implementations;
using Infrastructure.Security.Interfaces;
using Infrastructure.Services.Implementations;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Context;

namespace Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
    
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql => npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            )
        );


        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserFactory, UserFactory>();
        services.AddScoped<IRoomFactory, RoomFactory>();
        services.AddScoped<ISeasonFactory, SeasonFactory>();
        services.AddScoped<IRateFactory, RateFactory>();
        services.AddScoped<IBookingFactory, BookingFactory>();
        services.AddScoped<IPaymentFactory, PaymentFactory>();
        services.AddScoped<IAdditionalServiceFactory, AdditionalServiceFactory>();
        services.AddScoped<IBookingServiceFactory, BookingServiceFactory>();
        services.AddScoped<IAuditLogFactory, AuditLogFactory>();

        services.AddScoped<IUserDomainService, UserDomainService>();
        services.AddScoped<IRoomDomainService, RoomDomainService>();
        services.AddScoped<IBookingDomainService, BookingDomainService>();
        services.AddScoped<IPaymentDomainService, PaymentDomainService>();
        services.AddScoped<ISeasonDomainService, SeasonDomainService>();
        services.AddScoped<IRateDomainService, RateDomainService>();
        services.AddScoped<IAdditionalServiceDomainService, AdditionalServiceDomainService>();

        services.AddScoped<ILoggerService, LoggerService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ICodeGeneratorService, CodeGeneratorService>();

        return services;
    }
}