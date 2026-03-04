using Alti.Domain.Entities;
using Alti.Domain.Interfaces.Repositories;
using Core.domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Implementations;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.Payments.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Payment?> GetByExternalReferenceAsync(string reference, CancellationToken ct = default)
        => await _context.Payments.FirstOrDefaultAsync(p => p.ExternalReference == reference, ct);

    public async Task<IReadOnlyList<Payment>> GetByBookingAsync(int bookingId, CancellationToken ct = default)
        => await _context.Payments.Where(p => p.BookingId == bookingId).ToListAsync(ct);

    public async Task AddAsync(Payment payment, CancellationToken ct = default)
        => await _context.Payments.AddAsync(payment, ct);

    public void Update(Payment payment)
        => _context.Payments.Update(payment);
}