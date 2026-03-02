using Alti.Domain.Entities;
using Alti.Domain.Enums;

namespace Alti.Domain.Factories.Interfaces;

public interface IUserFactory
{
    User Create(
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        UserRole role = UserRole.Guest,
        string? phone = null);
}