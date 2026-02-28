using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Alti.Core.Domain.Entities.Booking;
using Alti.Core.Domain.Enums;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    public class PagoRepository : BaseRepository<Pago>, IPagoRepository
    {
        public PagoRepository(DbContext context) : base(context) { }

        public async Task<Pago?> ObtenerPorReferenciaAsync(string referenciaPago, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(referenciaPago)) return null;
            var normalized = referenciaPago.Trim();
            return await Query()
                .FirstOrDefaultAsync(p => p.ReferenciaPago == normalized, ct)
                .ConfigureAwait(false);
        }

        public async Task<Pago?> ObtenerPorReferenciaExternaAsync(string referenciaExterna, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(referenciaExterna)) return null;
            var normalized = referenciaExterna.Trim();
            return await Query()
                .FirstOrDefaultAsync(p => p.ReferenciaExterna == normalized, ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Pago>> ObtenerPorReservaAsync(int reservaId, CancellationToken ct = default)
        {
            return await Query()
                .Where(p => p.ReservaId == reservaId)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Pago>> ObtenerPorEstadoAsync(EstadoPago estado, CancellationToken ct = default)
        {
            return await Query()
                .Where(p => p.Estado == estado)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Pago>> ObtenerPorMetodoAsync(MetodoPago metodo, CancellationToken ct = default)
        {
            return await Query()
                .Where(p => p.MetodoPago == metodo)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<bool> CompletarAsync(int pagoId, string? referenciaExterna, CancellationToken ct = default)
        {
            var pago = await _dbSet.FindAsync(new object[] { pagoId }, ct).ConfigureAwait(false);
            if (pago is null) return false;

            pago.Completar(referenciaExterna);
            _dbSet.Update(pago);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> FallarAsync(int pagoId, string motivo, CancellationToken ct = default)
        {
            var pago = await _dbSet.FindAsync(new object[] { pagoId }, ct).ConfigureAwait(false);
            if (pago is null) return false;

            pago.Fallar(motivo);
            _dbSet.Update(pago);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> IniciarProcesamientoAsync(int pagoId, CancellationToken ct = default)
        {
            var pago = await _dbSet.FindAsync(new object[] { pagoId }, ct).ConfigureAwait(false);
            if (pago is null) return false;

            pago.IniciarProcesamiento();
            _dbSet.Update(pago);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> ReembolsarAsync(int pagoId, int autorizadoPorId, string motivo, CancellationToken ct = default)
        {
            var pago = await _dbSet.FindAsync(new object[] { pagoId }, ct).ConfigureAwait(false);
            if (pago is null) return false;

            var result = pago.Reembolsar(autorizadoPorId, motivo);
            if (!result.EsExitoso) return false;

            _dbSet.Update(pago);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }
    }
}
