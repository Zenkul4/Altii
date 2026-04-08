using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Factories.Interfaces;

namespace Alti.Domain.Factories.Implementations;

public class RoomFactory : IRoomFactory
{
    public Room Create(
        string number,
        RoomType type,
        short floor,
        short capacity,
        decimal basePrice,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Room number cannot be empty.", nameof(number));

        if (basePrice <= 0)
            throw new ArgumentException("Base price must be greater than zero.", nameof(basePrice));

        if (capacity is < 1 or > 20)
            throw new ArgumentException("Capacity must be between 1 and 20.", nameof(capacity));

        if (floor <= 1)
            throw new ArgumentException("Floor cannot be negative or zero.", nameof(floor));

        return new Room
        {
            Number = number.Trim().ToUpper(),
            Type = type,
            Floor = floor,
            Capacity = capacity,
            BasePrice = basePrice,
            Description = description?.Trim()
        };
    }
}