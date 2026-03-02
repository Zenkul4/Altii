using Alti.Domain.Entities;
using Alti.Domain.Enums;

namespace Alti.Domain.Factories.Interfaces;

public interface IRoomFactory
{
    Room Create(
        string number,
        RoomType type,
        short floor,
        short capacity,
        decimal basePrice,
        string? description = null);
}