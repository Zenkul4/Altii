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
        services.AddTransient<MainShellViewModel>(sp => new MainShellViewModel(
            sp.GetRequiredService<IAuthService>(),
            sp.GetRequiredService<DashboardViewModel>(),
            sp.GetRequiredService<BookingsViewModel>(),
            sp.GetRequiredService<PaymentsViewModel>(),
            sp.GetRequiredService<RoomsViewModel>(),
            sp.GetRequiredService<UsersViewModel>()
        ));

        // Views
        services.AddTransient<LoginView>();
        services.AddTransient<MainShellView>();

        Services = services.BuildServiceProvider();

        var login = Services.GetRequiredService<LoginView>();
        login.Show();
    }
}