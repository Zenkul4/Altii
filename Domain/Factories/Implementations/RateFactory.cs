using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Factories.Interfaces;

namespace Alti.Domain.Factories.Implementations;

public class RateFactory : IRateFactory
{
    public Rate Create(int seasonId, RoomType roomType, decimal pricePerNight, int createdById)
    {
        if (pricePerNight <= 0)
            throw new ArgumentException("Price per night must be greater than zero.", nameof(pricePerNight));

        return new Rate
        {
            SeasonId = seasonId,
            RoomType = roomType,
            PricePerNight = pricePerNight,
            CreatedById = createdById
        };
    }
}