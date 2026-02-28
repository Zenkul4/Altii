using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Alti.Core.Domain.Entities.Security;
using Alti.Core.Domain.Enums;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<Usuario>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context) { }

        public async Task<Usuario?> ObtenerPorEmailAsync(string email, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            var normalized = email.Trim().ToLowerInvariant();
            return await Query()
                .FirstOrDefaultAsync(u => u.Email == normalized, ct)
                .ConfigureAwait(false);
        }

        public async Task<bool> ExistePorEmailAsync(string email, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            var normalized = email.Trim().ToLowerInvariant();
            return await Query()
                .AnyAsync(u => u.Email == normalized, ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Usuario>> ObtenerPorRolAsync(int rolId, CancellationToken ct = default)
        {
            return await Query()
                .Where(u => u.RolId == rolId)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Usuario>> ObtenerPorEstadoAsync(EstadoUsuario estado, CancellationToken ct = default)
        {
            return await Query()
                .Where(u => u.Estado == estado)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<Usuario?> ActualizarPerfilAsync(int usuarioId, string nombre, string apellido, string? telefono, string? documento, CancellationToken ct = default)
        {
            var usuario = await _dbSet.FindAsync(new object[] { usuarioId }, ct).ConfigureAwait(false);
            if (usuario is null) return null;

            usuario.ActualizarPerfil(nombre, apellido, telefono, documento);
            _dbSet.Update(usuario);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);

            return usuario;
        }

        public async Task CambiarPasswordAsync(int usuarioId, string nuevoHash, CancellationToken ct = default)
        {
            var usuario = await _dbSet.FindAsync(new object[] { usuarioId }, ct).ConfigureAwait(false);
            if (usuario is null) return;

            usuario.CambiarPassword(nuevoHash);
            _dbSet.Update(usuario);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task CambiarEstadoAsync(int usuarioId, EstadoUsuario nuevoEstado, CancellationToken ct = default)
        {
            var usuario = await _dbSet.FindAsync(new object[] { usuarioId }, ct).ConfigureAwait(false);
            if (usuario is null) return;

            usuario.CambiarEstado(nuevoEstado);
            _dbSet.Update(usuario);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task CambiarRolAsync(int usuarioId, int nuevoRolId, CancellationToken ct = default)
        {
            var usuario = await _dbSet.FindAsync(new object[] { usuarioId }, ct).ConfigureAwait(false);
            if (usuario is null) return;

            usuario.CambiarRol(nuevoRolId);
            _dbSet.Update(usuario);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task RegistrarAccesoExitosoAsync(int usuarioId, CancellationToken ct = default)
        {
            var usuario = await _dbSet.FindAsync(new object[] { usuarioId }, ct).ConfigureAwait(false);
            if (usuario is null) return;

            usuario.RegistrarAccesoExitoso();
            _dbSet.Update(usuario);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task RegistrarAccesoFallidoAsync(int usuarioId, CancellationToken ct = default)
        {
            var usuario = await _dbSet.FindAsync(new object[] { usuarioId }, ct).ConfigureAwait(false);
            if (usuario is null) return;

            usuario.RegistrarAccesoFallido();
            _dbSet.Update(usuario);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Usuario>> ObtenerBloqueadosAsync(CancellationToken ct = default)
        {
            return await Query()
                .Where(u => u.EstaBloqueado())
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }
    }
}
