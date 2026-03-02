using Alti.Domain.Entities;

namespace Alti.Domain.Factories.Interfaces;

public interface IPaymentFactory
{
    Payment Create(int bookingId, decimal amount, string? paymentMethod = null);
}