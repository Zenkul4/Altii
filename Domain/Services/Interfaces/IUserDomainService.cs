using Alti.Domain.Entities;
using Alti.Domain.Enums;

namespace Alti.Domain.Services.Interfaces;

public interface IUserDomainService
{
    void UpdateDetails(User user, string firstName, string lastName, string? phone);
    void ChangePassword(User user, string newHash);
    void ChangeRole(User user, UserRole newRole);
    void Deactivate(User user);
    void Reactivate(User user);
    void VerifyRole(User user, UserRole requiredRole);
}