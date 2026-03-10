namespace Application.DTOs.BookingService;

public class BookingServiceResponseDto
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public short Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    public DateTimeOffset RegisteredAt { get; set; }
}
