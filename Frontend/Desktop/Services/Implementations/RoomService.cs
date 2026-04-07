using Desktop.Models.Room;
using Desktop.Services.Interfaces;
using static Desktop.Models.Room.RoomDto;

namespace Desktop.Services.Implementations;

public class RoomService : IRoomService
{
    private readonly ApiClient _api;

    public RoomService(ApiClient api) => _api = api;

    public async Task<List<RoomDto>> GetAllAsync()
    {
        try
        {
            return await _api.GetAsync<List<RoomDto>>("Rooms/all");
        }
        catch
        {
            return await _api.GetAsync<List<RoomDto>>(
                "Rooms/available?checkIn=2020-01-01&checkOut=2020-01-02");
        }
    }

    public Task<RoomDto> GetByIdAsync(int id) => _api.GetAsync<RoomDto>($"Rooms/{id}");
    public Task<RoomDto> CreateAsync(CreateRoomDto dto) => _api.PostAsync<RoomDto>("Rooms", dto);
    public Task<RoomDto> UpdateAsync(int id, UpdateRoomDto dto) => _api.PutAsync<RoomDto>($"Rooms/{id}", dto);
    public Task DisableAsync(int id) => _api.PatchAsync($"Rooms/{id}/disable");
    public Task EnableAsync(int id) => _api.PatchAsync($"Rooms/{id}/enable");
    public Task MarkAvailableAsync(int id) => _api.PatchAsync($"Rooms/{id}/mark-available");
    public Task MarkBlockedAsync(int id) => _api.PatchAsync($"Rooms/{id}/mark-blocked");
    public Task ReleaseBlockAsync(int id) => _api.PatchAsync($"Rooms/{id}/release-block");
    public Task MarkOccupiedAsync(int id) => _api.PatchAsync($"Rooms/{id}/mark-occupied");
    public Task MarkCleaningAsync(int id) => _api.PatchAsync($"Rooms/{id}/mark-cleaning");

}