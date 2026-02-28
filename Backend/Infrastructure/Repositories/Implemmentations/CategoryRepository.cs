using Alti.Core.Domain.Entities.Hotel;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Categoria>, ICategoryRepository
    {
        public CategoryRepository(DbContext context) : base(context) { }

        public async Task<Categoria?> ObtenerPorNombreAsync(string nombre, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return null;
            var normalized = nombre.Trim();
            return await Query()
                .FirstOrDefaultAsync(c => c.Nombre == normalized, ct)
                .ConfigureAwait(false);
        }

        public async Task<bool> ExistePorNombreAsync(string nombre, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return false;
            var normalized = nombre.Trim();
            return await Query()
                .AnyAsync(c => c.Nombre == normalized, ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Categoria>> BuscarPorTerminoAsync(string termino, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(termino))
                return Array.Empty<Categoria>();

            var term = termino.Trim();
            return await Query()
                .Where(c => EF.Functions.Like(c.Nombre, $"%{term}%") ||
                            EF.Functions.Like(c.Descripcion ?? string.Empty, $"%{term}%"))
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<Categoria?> ObtenerConHabitacionesAsync(int categoriaId, CancellationToken ct = default)
        {
            return await Query(asNoTracking: false)
                .Include(c => c.Habitaciones)
                .FirstOrDefaultAsync(c => c.Id == categoriaId, ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Categoria>> ObtenerPorCapacidadAsync(int capacidadMinima, CancellationToken ct = default)
        {
            return await Query()
                .Where(c => c.CapacidadMaxima >= capacidadMinima)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Categoria>> ObtenerPorPrecioMaximoAsync(decimal precioMaximo, CancellationToken ct = default)
        {
            return await Query()
                .Where(c => c.PrecioBaseNoche <= precioMaximo)
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }

        public async Task<(IReadOnlyList<Categoria> Items, int Total)> ObtenerPaginadoAsync(int skip, int take, string? termino = null, CancellationToken ct = default)
        {
            var q = Query();

            if (!string.IsNullOrWhiteSpace(termino))
            {
                var term = termino.Trim();
                q = q.Where(c => EF.Functions.Like(c.Nombre, $"%{term}%") ||
                                 EF.Functions.Like(c.Descripcion ?? string.Empty, $"%{term}%"));
            }

            var total = await q.CountAsync(ct).ConfigureAwait(false);
            var items = await q
                .OrderBy(c => c.Nombre)
                .Skip(skip)
                .Take(take)
                .ToListAsync(ct)
                .ConfigureAwait(false);

            return (items, total);
        }

        public async Task<IReadOnlyList<Categoria>> ObtenerPorIdsAsync(IEnumerable<int> ids, CancellationToken ct = default)
        {
            if (ids is null) return Array.Empty<Categoria>();
            var idList = ids as int[] ?? ids.ToArray();
            if (idList.Length == 0) return Array.Empty<Categoria>();

            return await Query()
                .Where(c => idList.Contains(c.Id))
                .ToListAsync(ct)
                .ConfigureAwait(false);
        }
    }
}
