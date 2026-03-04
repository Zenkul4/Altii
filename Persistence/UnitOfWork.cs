using Alti.Domain.Interfaces;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Context;
using Persistence.Repositories.Implementations;

namespace Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public IUserRepository Users { get; }
    public IRoomRepository Rooms { get; }
    public ISeasonRepository Seasons { get; }
    public IRateRepository Rates { get; }
    public IBookingRepository Bookings { get; }
    public IPaymentRepository Payments { get; }
    public IAdditionalServiceRepository AdditionalServices { get; }
    public IBookingServiceRepository BookingServices { get; }
    public IAuditLogRepository AuditLogs { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new UserRepository(context);
        Rooms = new RoomRepository(context);
        Seasons = new SeasonRepository(context);
        Rates = new RateRepository(context);
        Bookings = new BookingRepository(context);
        Payments = new PaymentRepository(context);
        AdditionalServices = new AdditionalServiceRepository(context);
        BookingServices = new BookingServiceRepository(context);
        AuditLogs = new AuditLogRepository(context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => _transaction = await _context.Database.BeginTransactionAsync(ct);

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction is null)
            throw new InvalidOperationException("No active transaction to commit.");

        await _transaction.CommitAsync(ct);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction is null)
            throw new InvalidOperationException("No active transaction to rollback.");

        await _transaction.RollbackAsync(ct);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}