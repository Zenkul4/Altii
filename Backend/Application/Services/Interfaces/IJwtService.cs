using Alti.Domain.Entities;

namespace Application.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
    DateTime GetExpiration();
}