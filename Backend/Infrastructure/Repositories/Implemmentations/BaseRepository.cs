using Alti.Infrastructure.Repositories.Interfaces;
using ALTI.Domain.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : AuditEntity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<T?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, ct).ConfigureAwait(false);
        }

        public virtual async Task<IReadOnlyList<T>> OtenerTodosAsync(CancellationToken ct = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(ct).ConfigureAwait(false);
        }

        public virtual async Task<IReadOnlyList<T>> BuscarAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(ct).ConfigureAwait(false);
        }

        public virtual async Task<T?> PrimerODefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, ct).ConfigureAwait(false);
        }

        public virtual async Task<bool> ExisteAsync(Expression<Func<T, bool>> predicado, CancellationToken ct = default)
        {
            return await _dbSet.AnyAsync(predicado, ct).ConfigureAwait(false);
        }

        public virtual async Task<int> ContarAsync(Expression<Func<T, bool>>? predicado = null, CancellationToken ct = default)
        {
            if (predicado is null)
                return await _dbSet.CountAsync(ct).ConfigureAwait(false);

            return await _dbSet.CountAsync(predicado, ct).ConfigureAwait(false);
        }

        public virtual async Task AgregarAsync(T entity, CancellationToken ct = default)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            await _dbSet.AddAsync(entity, ct).ConfigureAwait(false);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public virtual async Task AgregarRangoAsync(IEnumerable<T> entidades, CancellationToken ct = default)
        {
            if (entidades is null) throw new ArgumentNullException(nameof(entidades));

            await _dbSet.AddRangeAsync(entidades, ct).ConfigureAwait(false);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        protected IQueryable<T> Query(bool asNoTracking = true)
        {
            return asNoTracking ? _dbSet.AsNoTracking() : _dbSet;
        }

        public virtual async Task ActualizarAsync(T entity, CancellationToken ct = default)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            _dbSet.Update(entity);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public virtual async Task EliminarAsync(T entity, CancellationToken ct = default)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public virtual async Task EliminarPorIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await ObtenerPorIdAsync(id, ct).ConfigureAwait(false);
            if (entity is null) return;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}
