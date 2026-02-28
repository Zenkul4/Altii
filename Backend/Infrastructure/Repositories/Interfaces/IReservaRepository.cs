using Alti.Core.Domain.Entities.Booking;
using Alti.Core.Domain.Enums;
using Alti.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IReservaRepository : IRepository<Reserva>
    {
        Task<Reserva?> ObtenerPorCodigoAsync(string codigo, CancellationToken ct = default);

        Task<IReadOnlyList<Reserva>> ObtenerPorUsuarioAsync(int usuarioId, CancellationToken ct = default);


        Task<IReadOnlyList<Reserva>> ObtenerPorHabitacionAsync(int habitacionId, CancellationToken ct = default);

        Task<IReadOnlyList<Reserva>> ObtenerPorEstadoAsync(EstadoReserva estado, CancellationToken ct = default);

        Task<IReadOnlyList<Reserva>> ObtenerPorRangoFechasAsync(DateTime inicio, DateTime fin, CancellationToken ct = default);


        Task<bool> ConfirmarAsync(int reservaId, CancellationToken ct = default);


        Task<bool> HacerCheckInAsync(int reservaId, CancellationToken ct = default);


        Task<bool> HacerCheckOutAsync(int reservaId, CancellationToken ct = default);

        Task<bool> CancelarAsync(int reservaId, string motivo, CancellationToken ct = default);


        Task<IReadOnlyList<Reserva>> ObtenerPendientesDePagoAsync(CancellationToken ct = default);
    }
}
