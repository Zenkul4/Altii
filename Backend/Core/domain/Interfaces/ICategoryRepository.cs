using Alti.Core.Domain.Entities.Hotel;
using Domain.Interfaces;

namespace Core.domain.Interfaces;

public interface ICategoryRepository : IRepository<Categoria>
{
    Task<Categoria?> ObtenerPorNombreAsync(string nombre, CancellationToken ct = default);


    Task<bool> ExistePorNombreAsync(string nombre, CancellationToken ct = default);


    Task<IReadOnlyList<Categoria>> BuscarPorTerminoAsync(string termino, CancellationToken ct = default);

    Task<Categoria?> ObtenerConHabitacionesAsync(int categoriaId, CancellationToken ct = default);

    Task<IReadOnlyList<Categoria>> ObtenerPorCapacidadAsync(int capacidadMinima, CancellationToken ct = default);


    Task<IReadOnlyList<Categoria>> ObtenerPorPrecioMaximoAsync(decimal precioMaximo, CancellationToken ct = default);

    
    Task<(IReadOnlyList<Categoria> Items, int Total)> ObtenerPaginadoAsync(
        int skip, int take, string? termino = null, CancellationToken ct = default);


    Task<IReadOnlyList<Categoria>> ObtenerPorIdsAsync(IEnumerable<int> ids, CancellationToken ct = default);
}
