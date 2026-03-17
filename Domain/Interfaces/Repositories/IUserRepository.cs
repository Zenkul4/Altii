using Alti.Domain.Entities;
using Alti.Domain.Enums;

namespace Alti.Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<IReadOnlyList<User>> GetByRoleAsync(UserRole role, CancellationToken ct = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
}