using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alti.Core.Domain.Common;


/// Wrapper para resultados paginados.Necesario para escalar las consultas de listas sin cargar toda la base de datos. La API y el Desktop pueden solicitar páginas específicas con filtros.
public sealed class ResultadoPaginado<T>
{
    public IReadOnlyList<T> Items { get; }
    public int PaginaActual { get; }
    public int TotalPaginas { get; }
    public int TotalItems { get; }
    public int TamañoPagina { get; }
    public bool TieneSiguiente => PaginaActual < TotalPaginas;
    public bool TieneAnterior => PaginaActual > 1;

    public ResultadoPaginado(IReadOnlyList<T> items, int total, int pagina, int tamaño)
    {
        Items = items;
        TotalItems = total;
        PaginaActual = pagina;
        TamañoPagina = tamaño;
        TotalPaginas = (int)Math.Ceiling(total / (double)tamaño);
    }

    public static async Task<ResultadoPaginado<T>> CrearAsync(
        IQueryable<T> fuente, int pagina, int tamaño,
        CancellationToken ct = default)
    {
        pagina = Math.Max(1, pagina);
        tamaño = Math.Clamp(tamaño, 1, 100);

        var total = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                                   .CountAsync(fuente, ct);
        var items = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                                   .ToListAsync(fuente.Skip((pagina - 1) * tamaño).Take(tamaño), ct);

        return new ResultadoPaginado<T>(items, total, pagina, tamaño);
    }
}