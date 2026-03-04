using Alti.Domain.Common;
using Alti.Domain.Enums;

namespace Alti.Domain.Entities;

public class Booking : BaseEntity
{
    public Booking() { }

    public string Code { get; internal set; } = string.Empty;
    public int GuestId { get; internal set; }
    public int RoomId { get; internal set; }
    public int? AttendedById { get; internal set; }
    public DateOnly CheckInDate { get; internal set; }
    public DateOnly CheckOutDate { get; internal set; }
    public decimal PricePerNight { get; internal set; }
    public decimal TotalPrice { get; internal set; }
    public BookingStatus Status { get; internal set; } = BookingStatus.PendingPayment;
    public DateTimeOffset ExpiresAt { get; internal set; }
    public string? Notes { get; internal set; }
}
