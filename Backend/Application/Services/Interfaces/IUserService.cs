using Application.DTOs.User;

namespace Application.Services.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<UserResponseDto>> GetAllAsync(CancellationToken ct = default);
    Task<UserResponseDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default);
    Task<UserResponseDto> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct = default);
    Task DeactivateAsync(int id, CancellationToken ct = default);
    Task ReactivateAsync(int id, CancellationToken ct = default);
}