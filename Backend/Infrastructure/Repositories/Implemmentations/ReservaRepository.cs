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
    public class ReservaRepository : BaseRepository<Reserva>, IReservaRepository
    {
        public ReservaRepository(DbContext context) : base(context) { }

        public async Task<Reserva?> ObtenerPorCodigoAsync(string codigo, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(codigo)) return null;
            var normalized = codigo.Trim();
            return await Query()
                .FirstOrDefaultAsync(r => r.CodigoConfirmacion == normalized, ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Reserva>> ObtenerPorUsuarioAsync(int usuarioId, CancellationToken ct = default)
        {
            return await Query()
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Reserva>> ObtenerPorHabitacionAsync(int habitacionId, CancellationToken ct = default)
        {
            return await Query()
                .Where(r => r.HabitacionId == habitacionId)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Reserva>> ObtenerPorEstadoAsync(EstadoReserva estado, CancellationToken ct = default)
        {
            return await Query()
                .Where(r => r.Estado == estado)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Reserva>> ObtenerPorRangoFechasAsync(DateTime inicio, DateTime fin, CancellationToken ct = default)
        {
            return await Query()
                .Where(r => r.FechaEntrada >= inicio.Date && r.FechaEntrada <= fin.Date)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<bool> ConfirmarAsync(int reservaId, CancellationToken ct = default)
        {
            var reserva = await _dbSet.FindAsync(new object[] { reservaId }, ct).ConfigureAwait(false);
            if (reserva is null) return false;

            var result = reserva.Confirmar();
            if (!result.EsExitoso) return false;

            _dbSet.Update(reserva);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> HacerCheckInAsync(int reservaId, CancellationToken ct = default)
        {
            var reserva = await _dbSet.FindAsync(new object[] { reservaId }, ct).ConfigureAwait(false);
            if (reserva is null) return false;

            var result = reserva.HacerCheckIn();
            if (!result.EsExitoso) return false;

            _dbSet.Update(reserva);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> HacerCheckOutAsync(int reservaId, CancellationToken ct = default)
        {
            var reserva = await _dbSet.FindAsync(new object[] { reservaId }, ct).ConfigureAwait(false);
            if (reserva is null) return false;

            var result = reserva.HacerCheckOut();
            if (!result.EsExitoso) return false;

            _dbSet.Update(reserva);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> CancelarAsync(int reservaId, string motivo, CancellationToken ct = default)
        {
            var reserva = await _dbSet.FindAsync(new object[] { reservaId }, ct).ConfigureAwait(false);
            if (reserva is null) return false;

            var result = reserva.Cancelar(motivo);
            if (!result.EsExitoso) return false;

            _dbSet.Update(reserva);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<IReadOnlyList<Reserva>> ObtenerPendientesDePagoAsync(CancellationToken ct = default)
        {
            return await Query()
                .Where(r => r.PagoRequerido && !r.EstaPagada())
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }
    }
}
