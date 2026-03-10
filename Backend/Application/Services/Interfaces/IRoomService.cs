using Alti.Domain.Enums;
using Application.DTOs.Room;

namespace Application.Services.Interfaces;

public interface IRoomService
{
    Task<RoomResponseDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<RoomResponseDto>> GetAvailableAsync(DateOnly checkIn, DateOnly checkOut, RoomType? type = null, short? minCapacity = null, CancellationToken ct = default);
    Task<RoomResponseDto> CreateAsync(CreateRoomDto dto, CancellationToken ct = default);
    Task<RoomResponseDto> UpdateAsync(int id, UpdateRoomDto dto, CancellationToken ct = default);
    Task DisableAsync(int id, CancellationToken ct = default);
    Task EnableAsync(int id, CancellationToken ct = default);
}