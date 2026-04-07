using Desktop.Models.Room;
using static Desktop.Models.Room.RoomDto;

namespace Desktop.Services.Interfaces;

public interface IRoomService
{
    Task<List<RoomDto>> GetAllAsync();
    Task<RoomDto> GetByIdAsync(int id);
    Task<RoomDto> CreateAsync(CreateRoomDto dto);
    Task<RoomDto> UpdateAsync(int id, UpdateRoomDto dto);
    Task DisableAsync(int id);
    Task EnableAsync(int id);
    Task MarkAvailableAsync(int id);
    Task MarkBlockedAsync(int id);
    Task ReleaseBlockAsync(int id);
    Task MarkOccupiedAsync(int id);
    Task MarkCleaningAsync(int id);

}