using Application.DTOs.Room;

namespace Application.Interfaces;

public interface IRoomAdminService
{
    Task<IEnumerable<RoomResponseDto>> GetAllRoomsAsync();
    Task MarkOccupiedAsync(int id);
    Task MarkCleaningAsync(int id);
    Task MarkBlockedAsync(int id);
    Task ReleaseBlockAsync(int id);
}