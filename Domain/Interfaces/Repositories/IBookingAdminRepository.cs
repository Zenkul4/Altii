using Alti.Domain.Entities;

namespace Alti.Domain.Interfaces.Repositories;

public interface IBookingAdminRepository
{
    Task<IReadOnlyList<Booking>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default);
}