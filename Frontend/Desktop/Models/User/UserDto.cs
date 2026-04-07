namespace Desktop.Models.User;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int Role { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public string RoleLabel
    {
        get => Role switch { 0 => "Huésped", 1 => "Recepcionista", 2 => "Administrador", _ => "Desconocido" };
        private set { }
    }

    public string Initial
    {
        get => string.IsNullOrEmpty(FullName) ? "?" : FullName[0].ToString().ToUpper();
        private set { }
    }
}

public class CreateUserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int Role { get; set; } = 0;
}

public class UpdateUserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
}