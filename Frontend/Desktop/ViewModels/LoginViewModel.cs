using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Helpers;
using Desktop.Services;
using Desktop.Services.Interfaces;
using Desktop.Views;
using System.Windows;

namespace Desktop.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    public string LoginButtonText => IsLoading ? "Verificando..." : "Ingresar";

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    // Se llama desde BaseViewModel cuando IsLoading cambia
    protected override void OnLoadingStateChanged(bool isLoading)
        => OnPropertyChanged(nameof(LoginButtonText));

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            SetError("Por favor ingrese su correo y contraseña.");
            return;
        }

        IsLoading = true;
        ClearMessages();

        try
        {
            var user = await _authService.LoginAsync(Email, Password);
            SessionStore.CurrentUser = user;

            var shell = App.Services.GetService(typeof(MainShellView)) as MainShellView;
            shell!.Show();

            foreach (Window w in Application.Current.Windows)
            {
                if (w is LoginView) { w.Close(); break; }
            }
        }
        catch (ApiException ex) when (ex.StatusCode == 403)
        {
            SetError("Acceso denegado. Solo el personal del hotel puede ingresar.");
        }
        catch (ApiException ex) when (ex.StatusCode == 401)
        {
            SetError("Correo o contraseña incorrectos.");
        }
        catch (Exception ex)
        {
            SetError($"Error de conexión: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void Close()
    {
        Application.Current.Shutdown();
    }
}