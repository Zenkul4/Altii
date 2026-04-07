using Desktop.Models.Auth;
using Desktop.Services.Interfaces;

namespace Desktop.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ApiClient _api;
    public LoginResponse? CurrentUser { get; private set; }

    public AuthService(ApiClient api) => _api = api;

    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var response = await _api.PostAsync<LoginResponse>("Auth/login", new LoginRequest
        {
            Email = email,
            Password = password,
        });

        if (!response.IsStaff)
            throw new ApiException(403, "Solo el personal del hotel puede acceder a esta aplicación.");

        CurrentUser = response;
        _api.SetToken(response.Token);
        return response;
    }

    public void Logout()
    {
        CurrentUser = null;
        _api.ClearToken();
    }
}