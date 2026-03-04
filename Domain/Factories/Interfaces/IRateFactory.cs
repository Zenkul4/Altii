using Alti.Domain.Entities;
using Alti.Domain.Enums;

namespace Alti.Domain.Factories.Interfaces;

public interface IRateFactory
{
    Rate Create(int seasonId, RoomType roomType, decimal pricePerNight, int createdById);
}