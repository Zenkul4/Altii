using Alti.Domain.Entities;

namespace Alti.Domain.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Payment?> GetByExternalReferenceAsync(string reference, CancellationToken ct = default);
    Task<IReadOnlyList<Payment>> GetByBookingAsync(int bookingId, CancellationToken ct = default);
    Task AddAsync(Payment payment, CancellationToken ct = default);
    void Update(Payment payment);
}