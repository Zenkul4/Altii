using Alti.Domain.Interfaces.Repositories;
using Core.domain.Interfaces.Repositories;

namespace Alti.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IRoomRepository Rooms { get; }
    ISeasonRepository Seasons { get; }
    IRateRepository Rates { get; }
    IBookingRepository Bookings { get; }
    IPaymentRepository Payments { get; }
    IAdditionalServiceRepository AdditionalServices { get; }
    IBookingServiceRepository BookingServices { get; }
    IAuditLogRepository AuditLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
}