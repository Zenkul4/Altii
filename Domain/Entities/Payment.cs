using Alti.Domain.Common;
using Alti.Domain.Enums;

namespace Alti.Domain.Entities;

public class Payment : BaseEntity
{
    internal Payment() { }

    public int BookingId { get; internal set; }
    public decimal Amount { get; internal set; }
    public PaymentStatus Status { get; internal set; } = PaymentStatus.Pending;
    public string? ExternalReference { get; internal set; }
    public string? PaymentMethod { get; internal set; }
    public DateTimeOffset? ProcessedAt { get; internal set; }
}