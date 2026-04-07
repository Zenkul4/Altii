namespace Desktop.Models.Payment;

public class PaymentDto
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public int Status { get; set; }
    public string? ExternalReference { get; set; }
    public string? PaymentMethod { get; set; }
    public DateTimeOffset? ProcessedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public string StatusLabel
    {
        get => Status switch
        {
            0 => "Pendiente",
            1 => "Aprobado",
            2 => "Rechazado",
            3 => "Reembolsado",
            _ => $"Estado {Status}"
        };
        private set { }
    }

    public string StatusColor
    {
        get => Status switch
        {
            0 => "#F59E0B",
            1 => "#10B981",
            2 => "#EF4444",
            3 => "#8B5CF6",
            _ => "#6B7280"
        };
        private set { }
    }

    public bool CanApprove { get => Status == 0; private set { } }
    public bool CanReject { get => Status == 0; private set { } }
    public bool CanRefund { get => Status == 1; private set { } }
}