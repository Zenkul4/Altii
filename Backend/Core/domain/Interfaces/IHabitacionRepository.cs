using Alti.Core.Domain.Entities.Hotel;
using Alti.Core.Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface IHabitacionRepository : IRepository<Habitacion>
{
    Task<Habitacion?> ObtenerPorNumeroAsync(string numero, CancellationToken ct = default);
    Task<IReadOnlyList<Habitacion>> ObtenerPorPisoAsync(int piso, CancellationToken ct = default);
    Task<IReadOnlyList<Habitacion>> ObtenerPorCategoriaAsync(int categoriaId, CancellationToken ct = default);
    Task<IReadOnlyList<Habitacion>> ObtenerPorEstadoAsync(EstadoHabitacion estado, CancellationToken ct = default);
    Task<IReadOnlyList<Habitacion>> ObtenerActivasAsync(CancellationToken ct = default);

    Task<bool> MarcarOcupadaAsync(int habitacionId, CancellationToken ct = default);
    Task<bool> MarcarDisponibleAsync(int habitacionId, CancellationToken ct = default);
    Task<bool> EnviarALimpiezaAsync(int habitacionId, CancellationToken ct = default);
    Task<bool> EnviarAMantenimientoAsync(int habitacionId, CancellationToken ct = default);
    Task<bool> DesactivarServicioAsync(int habitacionId, CancellationToken ct = default);

    Task<bool> RegistrarLimpiezaAsync(int habitacionId, int empleadoId, CancellationToken ct = default);
    Task<bool> ActivarAsync(int habitacionId, CancellationToken ct = default);
    Task<bool> DesactivarAsync(int habitacionId, CancellationToken ct = default);
    Task<bool> ActualizarNotasAsync(int habitacionId, string? notas, CancellationToken ct = default);
}
