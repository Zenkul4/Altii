namespace Desktop.Models.AdditionalService;

public class AdditionalServiceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }

    public string StatusLabel => IsActive ? "Activo" : "Inactivo";
    public string StatusColor => IsActive ? "#86EFAC" : "#FCA5A5";
}

public class CreateAdditionalServiceDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
}

public class BookingServiceDto
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
}

public class CreateBookingServiceDto
{
    public int BookingId { get; set; }
    public int ServiceId { get; set; }
}
public class UpdateAdditionalServiceDto
{
    public decimal Price { get; set; }
    public string? Description { get; set; }
}