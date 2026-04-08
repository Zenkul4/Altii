using System.Windows;
using Desktop.Services;
using Desktop.Services.Implementations;
using Desktop.Services.Interfaces;
using Desktop.ViewModels;
using Desktop.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        // Core
        services.AddSingleton<ApiClient>();

        // Services
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IBookingService, BookingService>();
        services.AddSingleton<IPaymentService, PaymentService>();
        services.AddSingleton<IRoomService, RoomService>();
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<ISeasonService, SeasonService>();
        services.AddSingleton<IRateService, RateService>();
        services.AddSingleton<IAdditionalServiceService, AdditionalServiceService>();


        // ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<DashboardViewModel>(sp => new DashboardViewModel(
            sp.GetRequiredService<IBookingService>(),
            sp.GetRequiredService<IRoomService>(),
            sp.GetRequiredService<IUserService>()
        ));
        services.AddTransient<BookingsViewModel>(sp => new BookingsViewModel(
            sp.GetRequiredService<IBookingService>(),
            sp.GetRequiredService<IPaymentService>(),
            sp.GetRequiredService<IAuthService>()
        ));
        services.AddTransient<PaymentsViewModel>(sp => new PaymentsViewModel(
            sp.GetRequiredService<IPaymentService>(),
            sp.GetRequiredService<IBookingService>()
        ));
        services.AddTransient<RoomsViewModel>(sp => new RoomsViewModel(
            sp.GetRequiredService<IRoomService>()
        ));
        services.AddTransient<UsersViewModel>(sp => new UsersViewModel(
            sp.GetRequiredService<IUserService>()
        ));
        services.AddTransient<SeasonsViewModel>(sp => new SeasonsViewModel(
            sp.GetRequiredService<ISeasonService>(),
            sp.GetRequiredService<IAuthService>()
        ));
        services.AddTransient<RatesViewModel>(sp => new RatesViewModel(
            sp.GetRequiredService<IRateService>(),
            sp.GetRequiredService<ISeasonService>(),
            sp.GetRequiredService<IAuthService>()
        ));
        services.AddTransient<ServicesViewModel>(sp => new ServicesViewModel(
            sp.GetRequiredService<IAdditionalServiceService>()
        ));
        services.AddTransient<MainShellViewModel>(sp => new MainShellViewModel(
            sp.GetRequiredService<IAuthService>(),
            sp.GetRequiredService<DashboardViewModel>(),
            sp.GetRequiredService<BookingsViewModel>(),
            sp.GetRequiredService<PaymentsViewModel>(),
            sp.GetRequiredService<RoomsViewModel>(),
            sp.GetRequiredService<UsersViewModel>(),
            sp.GetRequiredService<SeasonsViewModel>(),
            sp.GetRequiredService<RatesViewModel>(),
            sp.GetRequiredService<ServicesViewModel>()
        ));
        // Views
        services.AddTransient<LoginView>();
        services.AddTransient<MainShellView>();

        Services = services.BuildServiceProvider();

        var login = Services.GetRequiredService<LoginView>();
        login.Show();
    }
}