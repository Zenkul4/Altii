namespace Application.DTOs.BookingService;

public class CreateBookingServiceDto
{
    public int BookingId { get; set; }
    public int ServiceId { get; set; }
    public int RegisteredById { get; set; }
    public short Quantity { get; set; }
}