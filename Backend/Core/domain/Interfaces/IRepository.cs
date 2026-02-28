using Domain.Interfaces;
using System.Linq.Expressions;


namespace Domain.Interfaces;

public interface IRepository<T>
{
    Task<T?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<T>> OtenerTodosAsync(CancellationToken ct = default);

    Task<IReadOnlyList<T>> BuscarAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

    Task<T?> PrimerODefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

    Task<bool> ExisteAsync(Expression<Func<T, bool>> predicado,
                            CancellationToken ct = default);
    Task<int> ContarAsync(Expression<Func<T, bool>>? predicado = null,
                            CancellationToken ct = default);

    Task AgregarAsync(T entity, CancellationToken ct = default);
    Task AgregarRangoAsync(IEnumerable<T> entidades, CancellationToken ct = default);
}