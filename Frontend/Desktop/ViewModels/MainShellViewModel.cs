using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Helpers;
using Desktop.Services.Interfaces;
using System.Net.Http; // Para manejo de errores de red si fuera necesario
using System.Windows;

namespace Desktop.ViewModels;

public partial class MainShellViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly DashboardViewModel _dashboardVm;
    private readonly BookingsViewModel _bookingsVm;
    private readonly PaymentsViewModel _paymentsVm;
    private readonly RoomsViewModel _roomsVm;
    private readonly UsersViewModel _usersVm;
    private readonly SeasonsViewModel _seasonsVm;
    private readonly RatesViewModel _ratesVm;
    private readonly ServicesViewModel _servicesVm;

    [ObservableProperty]
    private BaseViewModel _currentPage;

    [ObservableProperty]
    private string _currentPageTitle = "Dashboard";

    [ObservableProperty]
    private string _selectedMenu = "Dashboard";

    public string UserName => SessionStore.CurrentUser?.FullName ?? string.Empty;
    public string UserRole => SessionStore.CurrentUser?.RoleName ?? string.Empty;
    public string UserInitial => UserName.Length > 0 ? UserName[0].ToString().ToUpper() : "?";
    public bool IsAdmin => SessionStore.IsAdmin;

    public MainShellViewModel(
        IAuthService authService,
        DashboardViewModel dashboardVm,
        BookingsViewModel bookingsVm,
        PaymentsViewModel paymentsVm,
        RoomsViewModel roomsVm,
        UsersViewModel usersVm,
        SeasonsViewModel seasonsVm,
        RatesViewModel ratesVm,
        ServicesViewModel servicesVm)
    {
        _authService = authService;
        _dashboardVm = dashboardVm;
        _bookingsVm = bookingsVm;
        _paymentsVm = paymentsVm;
        _roomsVm = roomsVm;
        _usersVm = usersVm;
        _seasonsVm = seasonsVm;
        _ratesVm = ratesVm;
        _servicesVm = servicesVm;

        _currentPage = dashboardVm;
    }

    [RelayCommand]
    private void NavigateTo(string page)
    {
        SelectedMenu = page;
        CurrentPageTitle = page switch
        {
            "Dashboard" => "Dashboard",
            "Bookings" => "Gestión de Reservas",
            "Payments" => "Gestión de Pagos",
            "Rooms" => "Habitaciones",
            "Users" => "Usuarios",
            "Seasons" => "Temporadas",
            "Rates" => "Tarifas",
            "Services" => "Servicios Adicionales",
            _ => page
        };

        CurrentPage = page switch
        {
            "Dashboard" => _dashboardVm,
            "Bookings" => _bookingsVm,
            "Payments" => _paymentsVm,
            "Rooms" => _roomsVm,
            "Users" => _usersVm,
            "Seasons" => _seasonsVm,
            "Rates" => _ratesVm,
            "Services" => _servicesVm,
            _ => _dashboardVm
        };

        // Al navegar, limpiamos mensajes de error previos de la vista base
        ClearMessages();
    }

    [RelayCommand]
    private void Logout()
    {
        try
        {
            _authService.Logout();
            SessionStore.CurrentUser = null;

            var login = App.Services.GetService(typeof(Desktop.Views.LoginView)) as Desktop.Views.LoginView;
            login!.Show();

            foreach (Window w in Application.Current.Windows)
            {
                if (w is Desktop.Views.MainShellView) { w.Close(); break; }
            }
        }
        catch (Exception ex)
        {
            // Error al intentar cerrar sesión o manipular ventanas
            MessageBox.Show($"Error al cerrar sesión: {ex.Message}", "ALTI System", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}