using Application.Services.Implementations;
using Application.Services.Interfaces;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<ISeasonService, SeasonService>();
        services.AddScoped<IRateService, RateService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IAdditionalServiceService, AdditionalServiceService>();
        services.AddScoped<IBookingServiceService, BookingServiceService>();

        services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

        return services;
    }
}