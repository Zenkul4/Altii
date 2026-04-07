using Desktop.Models.User;
using Desktop.Services.Interfaces;

namespace Desktop.Services.Implementations;

public class UserService : IUserService
{
    private readonly ApiClient _api;
    public UserService(ApiClient api) => _api = api;

    public Task<List<UserDto>> GetAllAsync() => _api.GetAsync<List<UserDto>>("Users");
    public Task<UserDto> GetByIdAsync(int id) => _api.GetAsync<UserDto>($"Users/{id}");
    public Task<UserDto> CreateAsync(CreateUserDto dto) => _api.PostAsync<UserDto>("Users", dto);
    public Task<UserDto> UpdateAsync(int id, UpdateUserDto dto) => _api.PutAsync<UserDto>($"Users/{id}", dto);
    public Task DeactivateAsync(int id) => _api.PatchAsync($"Users/{id}/deactivate");
    public Task ReactivateAsync(int id) => _api.PatchAsync($"Users/{id}/reactivate");
}