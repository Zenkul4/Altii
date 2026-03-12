using Alti.Domain.Exceptions;
using Alti.Domain.Factories.Interfaces;
using Alti.Domain.Interfaces;
using Alti.Domain.Services.Interfaces;
using Application.Dtos.User;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Mappers;
using Application.Services.Interfaces;
using Infrastructure.Security.Interfaces;

namespace Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserFactory _factory;
    private readonly IUserDomainService _domainService;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        IUnitOfWork uow,
        IUserFactory factory,
        IUserDomainService domainService,
        IPasswordHasher passwordHasher)
    {
        _uow = uow;
        _factory = factory;
        _domainService = domainService;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var user = await _uow.Users.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"User {id} not found.");

        return UserMapper.ToDto(user);
    }

    public async Task<IReadOnlyList<UserResponseDto>> GetAllAsync(CancellationToken ct = default)
    {
        var users = await _uow.Users.GetByRoleAsync(Alti.Domain.Enums.UserRole.Guest, ct);
        return users.Select(UserMapper.ToDto).ToList();
    }

    public async Task<UserResponseDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default)
    {
        if (await _uow.Users.EmailExistsAsync(dto.Email, ct))
            throw new DuplicateEmailException(dto.Email);

        var hash = _passwordHasher.Hash(dto.Password);
        var user = _factory.Create(dto.FirstName, dto.LastName, dto.Email, hash, dto.Role, dto.Phone);

        await _uow.Users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return UserMapper.ToDto(user);
    }

    public async Task<UserResponseDto> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct = default)
    {
        var user = await _uow.Users.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"User {id} not found.");

        _domainService.UpdateDetails(user, dto.FirstName, dto.LastName, dto.Phone);
        _uow.Users.Update(user);
        await _uow.SaveChangesAsync(ct);

        return UserMapper.ToDto(user);
    }

    public async Task DeactivateAsync(int id, CancellationToken ct = default)
    {
        var user = await _uow.Users.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"User {id} not found.");

        _domainService.Deactivate(user);
        _uow.Users.Update(user);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task ReactivateAsync(int id, CancellationToken ct = default)
    {
        var user = await _uow.Users.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"User {id} not found.");

        _domainService.Reactivate(user);
        _uow.Users.Update(user);
        await _uow.SaveChangesAsync(ct);
    }
}