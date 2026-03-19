using Alti.Domain.Enums;
using Alti.Domain.Exceptions;
using Alti.Domain.Factories.Interfaces;
using Alti.Domain.Interfaces;
using Alti.Domain.Services.Interfaces;
using Application.Dtos.Auth;
using Application.Dtos.User;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Mappers;
using Application.Services.Interfaces;

namespace Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserFactory _factory;
    private readonly IUserDomainService _domainService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwt;

    public AuthService(
        IUnitOfWork uow,
        IUserFactory factory,
        IUserDomainService domainService,
        IPasswordHasher passwordHasher,
        IJwtService jwt)
    {
        _uow = uow;
        _factory = factory;
        _domainService = domainService;
        _passwordHasher = passwordHasher;
        _jwt = jwt;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto, CancellationToken ct = default)
    {
        var user = await _uow.Users.GetByEmailAsync(dto.Email, ct);

        if (user is null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales incorrectas.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Credenciales incorrectas.");

        var token = _jwt.GenerateToken(user);
        var expires = _jwt.GetExpiration();

        return new LoginResponseDto
        {
            Id = user.Id,
            FullName = $"{user.FirstName} {user.LastName}",
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive,
            Token = token,
            ExpiresAt = expires,
        };
    }

    public async Task<UserResponseDto> RegisterAsync(CreateUserDto dto, CancellationToken ct = default)
    {
        if (await _uow.Users.EmailExistsAsync(dto.Email, ct))
            throw new DuplicateEmailException(dto.Email);

        var hash = _passwordHasher.Hash(dto.Password);
        var user = _factory.Create(
            dto.FirstName, dto.LastName,
            dto.Email, hash,
            UserRole.Guest, dto.Phone);

        await _uow.Users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return UserMapper.ToDto(user);
    }
}