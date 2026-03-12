using Alti.Domain.Entities;

namespace Alti.Domain.Interfaces.Repositories;

public interface IPaymentRepository : IBaseRepository<Payment>
{
    Task<Payment?> GetByExternalReferenceAsync(string reference, CancellationToken ct = default);
    Task<IReadOnlyList<Payment>> GetByBookingAsync(int bookingId, CancellationToken ct = default);
}