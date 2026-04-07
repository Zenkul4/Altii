namespace Desktop.Models.Booking;

public class BookingDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public int GuestId { get; set; }
    public string GuestFullName { get; set; } = string.Empty;
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public int Nights { get; set; }
    public decimal PricePerNight { get; set; }
    public decimal TotalPrice { get; set; }
    public int Status { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public string StatusLabel
    {
        get => Status switch
        {
            0 => "Pago pendiente",
            1 => "Confirmada",
            2 => "En hotel",
            3 => "Completada",
            4 => "Cancelada",
            5 => "Expirada",
            _ => $"Estado {Status}"
        };
        private set { }
    }

    public string StatusColor
    {
        get => Status switch
        {
            0 => "#F59E0B",
            1 => "#3B82F6",
            2 => "#10B981",
            3 => "#6B7280",
            4 => "#EF4444",
            5 => "#9CA3AF",
            _ => "#6B7280"
        };
        private set { }
    }

    public bool CanConfirm { get => Status == 0; private set { } }
    public bool CanCheckIn { get => Status == 1; private set { } }
    public bool CanCheckOut { get => Status == 2; private set { } }
    public bool CanCancel { get => Status is 0 or 1; private set { } }
}


