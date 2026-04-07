using Alti.Domain.Interfaces;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repositories.Implementations;

namespace Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration          configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql => npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            )
        );

        services.AddScoped<IUnitOfWork,                  UnitOfWork>();
        services.AddScoped<IUserRepository,              UserRepository>();
        services.AddScoped<IRoomRepository,              RoomRepository>();
        services.AddScoped<ISeasonRepository,            SeasonRepository>();
        services.AddScoped<IRateRepository,              RateRepository>();
        services.AddScoped<IBookingRepository,           BookingRepository>();
        services.AddScoped<IPaymentRepository,           PaymentRepository>();
        services.AddScoped<IAdditionalServiceRepository, AdditionalServiceRepository>();
        services.AddScoped<IBookingServiceRepository,    BookingServiceRepository>();
        services.AddScoped<IAuditLogRepository,          AuditLogRepository>();
        services.AddScoped<IRoomAdminRepository,         RoomRepository>();
        services.AddScoped<IBookingAdminRepository,      BookingRepository>();
        return services;
    }
}
