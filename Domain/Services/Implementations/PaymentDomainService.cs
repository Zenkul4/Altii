using Alti.Domain.Entities;
using Alti.Domain.Enums;
using Alti.Domain.Services.Interfaces;

namespace Alti.Domain.Services.Implementations;

public class PaymentDomainService : IPaymentDomainService
{
    public void Approve(Payment payment, string externalReference)
    {
        if (string.IsNullOrWhiteSpace(externalReference))
            throw new ArgumentException("External reference is required when approving.", nameof(externalReference));

        payment.Status = PaymentStatus.Approved;
        payment.ExternalReference = externalReference.Trim();
        payment.ProcessedAt = DateTimeOffset.UtcNow;
        payment.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Reject(Payment payment)
    {
        payment.Status = PaymentStatus.Rejected;
        payment.ProcessedAt = DateTimeOffset.UtcNow;
        payment.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Refund(Payment payment)
    {
        if (payment.Status != PaymentStatus.Approved)
            throw new InvalidOperationException("Only approved payments can be refunded.");

        payment.Status = PaymentStatus.Refunded;
        payment.UpdatedAt = DateTimeOffset.UtcNow;
    }
}