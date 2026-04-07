namespace Desktop.Models.Auth;

public class LoginResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Role { get; set; }
    public bool IsActive { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }

    public string RoleName => Role switch
    {
        0 => "Huésped",
        1 => "Recepcionista",
        2 => "Administrador",
        _ => "Desconocido"
    };

    public bool IsAdmin => Role == 2;
    public bool IsReceptionist => Role == 1;
    public bool IsStaff => Role >= 1;
}