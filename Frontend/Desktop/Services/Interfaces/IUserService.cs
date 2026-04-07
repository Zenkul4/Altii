using Desktop.Models.User;

namespace Desktop.Services.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(int id);
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task<UserDto> UpdateAsync(int id, UpdateUserDto dto);
    Task DeactivateAsync(int id);
    Task ReactivateAsync(int id);
}