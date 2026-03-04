using Alti.Domain.Entities;
using Alti.Domain.Factories.Interfaces;

namespace Alti.Domain.Factories.Implementations;

public class PaymentFactory : IPaymentFactory
{
    public Payment Create(int bookingId, decimal amount, string? paymentMethod = null)
    {
        if (amount <= 0)
            throw new ArgumentException("Payment amount must be greater than zero.", nameof(amount));

        return new Payment
        {
            BookingId = bookingId,
            Amount = amount,
            PaymentMethod = paymentMethod?.Trim()
        };
    }
}