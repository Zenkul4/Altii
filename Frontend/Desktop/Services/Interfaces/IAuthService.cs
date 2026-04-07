using Desktop.Models.Auth;

namespace Desktop.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(string email, string password);
    LoginResponse? CurrentUser { get; }
    void Logout();
}