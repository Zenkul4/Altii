using Application.Dtos.Auth;
using Application.Dtos.User;
using Application.DTOs.User;

namespace Application.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto dto, CancellationToken ct = default);
    Task<UserResponseDto> RegisterAsync(CreateUserDto dto, CancellationToken ct = default);
}