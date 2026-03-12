using Alti.Domain.Entities;
using Alti.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context) : base(context) { }

    public async Task<Payment?> GetByExternalReferenceAsync(string reference, CancellationToken ct = default)
        => await DbSet.FirstOrDefaultAsync(p => p.ExternalReference == reference, ct);

    public async Task<IReadOnlyList<Payment>> GetByBookingAsync(int bookingId, CancellationToken ct = default)
        => await DbSet.Where(p => p.BookingId == bookingId).ToListAsync(ct);
}