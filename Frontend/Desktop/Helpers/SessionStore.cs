using Desktop.Models.Auth;

namespace Desktop.Helpers;

public static class SessionStore
{
    public static LoginResponse? CurrentUser { get; set; }
    public static bool IsLoggedIn => CurrentUser is not null;
    public static bool IsAdmin => CurrentUser?.IsAdmin ?? false;
    public static bool IsStaff => CurrentUser?.IsStaff ?? false;
}