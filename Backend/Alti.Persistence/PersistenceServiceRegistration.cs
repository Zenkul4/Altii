using Alti.Domain.Interfaces;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Alti.Persistence.Context;
using Alti.Persistence.Repositories.Implementations;

namespace Alti.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration          configuration)
    {
        var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");

        services.AddDbContext<AppDbContext>(options =>
        {
            if (useInMemory)
            {
                options.UseInMemoryDatabase("AltiiDb");
            }
            else
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    npgsql => npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                );
            }
        });

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
        services.AddScoped<IAdditionalServiceAdminRepository, AdditionalServiceRepository>();
        return services;
    }
}

