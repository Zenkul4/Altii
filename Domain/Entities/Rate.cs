using Alti.Domain.Common;
using Alti.Domain.Enums;
using Core.domain.entities;

namespace Alti.Domain.Entities;

public class Rate : BaseEntity
{
    private Rate() { }

    public int SeasonId { get; private set; }
    public RoomType RoomType { get; private set; }
    public decimal PricePerNight { get; private set; }
    public int CreatedById { get; private set; }

    public Season Season { get; private set; } = null!;
    public User CreatedBy { get; private set; } = null!;

    public static Rate Create(int seasonId, RoomType roomType, decimal pricePerNight, int createdById)
    {
        if (pricePerNight <= 0)
            throw new ArgumentException("El precio por noche debe de ser mayor a 0", nameof(pricePerNight));

        return new Rate
        {
            SeasonId = seasonId,
            RoomType = roomType,
            PricePerNight = pricePerNight,
            CreatedById = createdById
        };
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("El precio debe de ser mayor a 0", nameof(newPrice));

        PricePerNight = newPrice;
        RegisterUpdate();
    }
}