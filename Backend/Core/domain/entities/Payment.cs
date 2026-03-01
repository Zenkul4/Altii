using Alti.Domain.Common;
using Alti.Domain.Enums;

namespace Alti.Domain.Entities;

public class Payment : BaseEntity
{
    private Payment() { }

    public int BookingId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
    public string? ExternalReference { get; private set; }
    public string? PaymentMethod { get; private set; }
    public DateTimeOffset? ProcessedAt { get; private set; }

    public Booking Booking { get; private set; } = null!;

    public bool WasApproved => Status == PaymentStatus.Approved;
    public bool CanBeRetried => Status is PaymentStatus.Rejected or PaymentStatus.Pending;

    public static Payment Create(int bookingId, decimal amount, string? paymentMethod = null)
    {
        if (amount <= 0)
            throw new ArgumentException("El monto de pago debe ser mayor a 0", nameof(amount));

        return new Payment
        {
            BookingId = bookingId,
            Amount = amount,
            PaymentMethod = paymentMethod?.Trim()
        };
    }

    public void Approve(string externalReference)
    {
        if (string.IsNullOrWhiteSpace(externalReference))
            throw new ArgumentException("Referencia externa es necesario para aprobar.", nameof(externalReference));

        Status = PaymentStatus.Approved;
        ExternalReference = externalReference.Trim();
        ProcessedAt = DateTimeOffset.UtcNow;
        RegisterUpdate();
    }

    public void Reject()
    {
        Status = PaymentStatus.Rejected;
        ProcessedAt = DateTimeOffset.UtcNow;
        RegisterUpdate();
    }

    public void Refund()
    {
        if (Status != PaymentStatus.Approved)
            throw new InvalidOperationException("Solo los pagos aprobado pueden ser reembolsados.");

        Status = PaymentStatus.Refunded;
        RegisterUpdate();
    }
}