using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Alti.Core.Domain.Entities.Hotel;
using Alti.Core.Domain.Enums;
using Infrastructure.Repositories.Interfaces;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class HabitacionRepository : BaseRepository<Habitacion>, IHabitacionRepository
    {
        public HabitacionRepository(DbContext context) : base(context) { }

        public async Task<Habitacion?> ObtenerPorNumeroAsync(string numero, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(numero)) return null;
            var normalized = numero.Trim().ToUpperInvariant();
            return await Query()
                .FirstOrDefaultAsync(h => h.Numero == normalized, ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Habitacion>> ObtenerPorPisoAsync(int piso, CancellationToken ct = default)
        {
            return await Query()
                .Where(h => h.Piso == piso)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Habitacion>> ObtenerPorCategoriaAsync(int categoriaId, CancellationToken ct = default)
        {
            return await Query()
                .Where(h => h.CategoriaId == categoriaId)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Habitacion>> ObtenerPorEstadoAsync(EstadoHabitacion estado, CancellationToken ct = default)
        {
            return await Query()
                .Where(h => h.Estado == estado)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Habitacion>> ObtenerActivasAsync(CancellationToken ct = default)
        {
            return await Query()
                .Where(h => h.Activa)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<bool> MarcarOcupadaAsync(int habitacionId, CancellationToken ct = default)
        {
            var h = await _dbSet.FindAsync(new object[] { habitacionId }, ct).ConfigureAwait(false);
            if (h is null) return false;
            h.MarcarOcupada();
            _dbSet.Update(h);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> MarcarDisponibleAsync(int habitacionId, CancellationToken ct = default)
        {
            var h = await _dbSet.FindAsync(new object[] { habitacionId }, ct).ConfigureAwait(false);
            if (h is null) return false;
            h.MarcarDisponible();
            _dbSet.Update(h);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> EnviarALimpiezaAsync(int habitacionId, CancellationToken ct = default)
        {
            var h = await _dbSet.FindAsync(new object[] { habitacionId }, ct).ConfigureAwait(false);
            if (h is null) return false;
            h.EnviarALimpieza();
            _dbSet.Update(h);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> EnviarAMantenimientoAsync(int habitacionId, CancellationToken ct = default)
        {
            var h = await _dbSet.FindAsync(new object[] { habitacionId }, ct).ConfigureAwait(false);
            if (h is null) return false;
            h.EnviarAMantenimiento();
            _dbSet.Update(h);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> DesactivarServicioAsync(int habitacionId, CancellationToken ct = default)
        {
            var h = await _dbSet.FindAsync(new object[] { habitacionId }, ct).ConfigureAwait(false);
            if (h is null) return false;
            h.DesactivarServicio();
            _dbSet.Update(h);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> RegistrarLimpiezaAsync(int habitacionId, int empleadoId, CancellationToken ct = default)
        {
            var h = await _dbSet.FindAsync(new object[] { habitacionId }, ct).ConfigureAwait(false);
            if (h is null) return false;
            h.RegistrarLimpieza(empleadoId);
            _dbSet.Update(h);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> ActivarAsync(int habitacionId, CancellationToken ct = default)
        {
            var h = await _dbSet.FindAsync(new object[] { habitacionId }, ct).ConfigureAwait(false);
            if (h is null) return false;
            h.Activar();
            _dbSet.Update(h);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> DesactivarAsync(int habitacionId, CancellationToken ct = default)
        {
            var h = await _dbSet.FindAsync(new object[] { habitacionId }, ct).ConfigureAwait(false);
            if (h is null) return false;
            h.Desactivar();
            _dbSet.Update(h);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> ActualizarNotasAsync(int habitacionId, string? notas, CancellationToken ct = default)
        {
            var h = await _dbSet.FindAsync(new object[] { habitacionId }, ct).ConfigureAwait(false);
            if (h is null) return false;
            h.ActualizarNotas(notas);
            _dbSet.Update(h);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }
    }
}
