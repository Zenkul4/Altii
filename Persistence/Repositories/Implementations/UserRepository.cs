using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class UserRepository : BaseRepository<User>, IUserRepository

{
    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default)
    => await DbSet.Where(u => u.IsActive).ToListAsync(ct);
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await DbSet.FirstOrDefaultAsync(u => u.Email == email.ToLower(), ct);

    public async Task<IReadOnlyList<User>> GetByRoleAsync(UserRole role, CancellationToken ct = default)
        => await DbSet.Where(u => u.Role == role && u.IsActive).ToListAsync(ct);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
        => await DbSet.AnyAsync(u => u.Email == email.ToLower(), ct);
}