namespace Desktop.Models.Booking;

public class CreateBookingDto
{
    public int GuestId { get; set; }
    public int RoomId { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public string? Notes { get; set; }
    public int? AttendedById { get; set; }
}