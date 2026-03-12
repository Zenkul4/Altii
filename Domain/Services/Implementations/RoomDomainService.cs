using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Exceptions;
using Alti.Domain.Services.Interfaces;

namespace Alti.Domain.Services.Implementations;

public class RoomDomainService : IRoomDomainService
{
    public void Block(Room room)
    {
        if (room.Status != RoomStatus.Available)
            throw new RoomNotAvailableException(room.Id, room.Status.ToString());

        room.Status = RoomStatus.Blocked;
        room.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void ReleaseBlock(Room room)
    {
        if (room.Status != RoomStatus.Blocked)
            throw new InvalidOperationException($"Room {room.Number} is not blocked.");

        room.Status = RoomStatus.Available;
        room.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkAsOccupied(Room room)
    {
        if (room.Status != RoomStatus.Blocked && room.Status != RoomStatus.Available)
            throw new RoomNotAvailableException(room.Id, room.Status.ToString());

        room.Status = RoomStatus.Occupied;
        room.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void SendToCleaning(Room room)
    {
        if (room.Status != RoomStatus.Occupied)
            throw new InvalidOperationException($"Only occupied rooms can be sent to cleaning.");

        room.Status = RoomStatus.Cleaning;
        room.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkAsAvailable(Room room)
    {
        if (room.Status == RoomStatus.Inactive)
            throw new InvalidOperationException($"Room {room.Number} is inactive.");

        room.Status = RoomStatus.Available;
        room.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Disable(Room room)
    {
        if (room.Status == RoomStatus.Occupied)
            throw new InvalidOperationException($"Room {room.Number} cannot be disabled while occupied.");

        room.Status = RoomStatus.Inactive;
        room.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Enable(Room room)
    {
        if (room.Status != RoomStatus.Inactive)
            throw new InvalidOperationException($"Room {room.Number} is not inactive.");

        room.Status = RoomStatus.Available;
        room.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateDetails(Room room, decimal newBasePrice, string? newDescription)
    {
        if (newBasePrice <= 0)
            throw new ArgumentException("Base price must be greater than zero.", nameof(newBasePrice));

        room.BasePrice = newBasePrice;
        room.Description = newDescription?.Trim();
        room.UpdatedAt = DateTimeOffset.UtcNow;
    }
}