using Alti.Core.Domain.Entities.Security;
using Alti.Core.Domain.Enums;
using Domain.Interfaces;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<Usuario>
    {
        Task<Usuario?> ObtenerPorEmailAsync(string email, CancellationToken ct = default);

        Task<bool> ExistePorEmailAsync(string email, CancellationToken ct = default);

        Task<IReadOnlyList<Usuario>> ObtenerPorRolAsync(int rolId, CancellationToken ct = default);

        Task<IReadOnlyList<Usuario>> ObtenerPorEstadoAsync(EstadoUsuario estado, CancellationToken ct = default);

        Task<Usuario?> ActualizarPerfilAsync(int usuarioId, string nombre, string apellido, string? telefono, string? documento, CancellationToken ct = default);

        Task CambiarPasswordAsync(int usuarioId, string nuevoHash, CancellationToken ct = default);

        Task CambiarEstadoAsync(int usuarioId, EstadoUsuario nuevoEstado, CancellationToken ct = default);

        Task CambiarRolAsync(int usuarioId, int nuevoRolId, CancellationToken ct = default);
        Task RegistrarAccesoExitosoAsync(int usuarioId, CancellationToken ct = default);
        Task RegistrarAccesoFallidoAsync(int usuarioId, CancellationToken ct = default);

        Task<IReadOnlyList<Usuario>> ObtenerBloqueadosAsync(CancellationToken ct = default);
    }
}
