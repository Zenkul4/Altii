using Alti.Domain.Common;

namespace Alti.Domain.Entities;

public class AdditionalService : BaseEntity
{
    private AdditionalService() { }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; } = true;

    public IReadOnlyCollection<BookingService> BookingServices
        => _bookingServices.AsReadOnly();

    private readonly List<BookingService> _bookingServices = new();

    public static AdditionalService Create(string name, decimal price, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del servicio no puede estar vacio", nameof(name));

        if (price < 0)
            throw new ArgumentException("El precio no puede ser negativo", nameof(price));

        return new AdditionalService
        {
            Name = name.Trim(),
            Price = price,
            Description = description?.Trim()
        };
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentException("El precio no puede ser negativo", nameof(newPrice));

        Price = newPrice;
        RegisterUpdate();
    }

    public void Deactivate() { IsActive = false; RegisterUpdate(); }
    public void Activate() { IsActive = true; RegisterUpdate(); }
}