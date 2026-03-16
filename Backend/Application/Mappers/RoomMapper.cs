using Alti.Domain.Entities;
using Application.DTOs.Room;

namespace Application.Mappers;

public static class RoomMapper
{
    public static RoomResponseDto ToDto(Room room) => new()
    {
        Id = room.Id,
        Number = room.Number,
        Type = room.Type,
        Floor = room.Floor,
        Capacity = room.Capacity,
        BasePrice = room.BasePrice,
        Description = room.Description,
        Status = room.Status,
        CreatedAt = room.CreatedAt
    };
}