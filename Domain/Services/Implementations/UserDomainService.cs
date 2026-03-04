using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Exceptions;
using Alti.Domain.Services.Interfaces;

namespace Alti.Domain.Services.Implementations;

public class UserDomainService : IUserDomainService
{
    public void UpdateDetails(User user, string firstName, string lastName, string? phone)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        user.FirstName = firstName.Trim();
        user.LastName = lastName.Trim();
        user.Phone = phone?.Trim();
        user.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void ChangePassword(User user, string newHash)
    {
        if (string.IsNullOrWhiteSpace(newHash))
            throw new ArgumentException("Password hash cannot be empty.", nameof(newHash));

        user.PasswordHash = newHash;
        user.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void ChangeRole(User user, UserRole newRole)
    {
        user.Role = newRole;
        user.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate(User user)
    {
        if (!user.IsActive)
            throw new InvalidOperationException($"User {user.Id} is already inactive.");

        user.IsActive = false;
        user.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Reactivate(User user)
    {
        if (user.IsActive)
            throw new InvalidOperationException($"User {user.Id} is already active.");

        user.IsActive = true;
        user.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void VerifyRole(User user, UserRole requiredRole)
    {
        if (user.Role != requiredRole)
            throw new AccessDeniedException(user.Id, requiredRole.ToString());
    }
}