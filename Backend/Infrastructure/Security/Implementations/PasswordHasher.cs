using Infrastructure.Security.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security.Implementations;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.", nameof(password));

        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }

    public bool Verify(string password, string hash)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.", nameof(password));

        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Hash cannot be empty.", nameof(hash));

        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}