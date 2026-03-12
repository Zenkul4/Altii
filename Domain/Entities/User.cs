using Alti.Domain.Common;
using Alti.Domain.Enums;

namespace Alti.Domain.Entities;

public class User : BaseEntity
{
    internal User() { }

    public string FirstName { get; internal set; } = string.Empty;
    public string LastName { get; internal set; } = string.Empty;
    public string Email { get; internal set; } = string.Empty;
    public string PasswordHash { get; internal set; } = string.Empty;
    public string? Phone { get; internal set; }
    public UserRole Role { get; internal set; } = UserRole.Guest;
    public bool IsActive { get; internal set; } = true;
}