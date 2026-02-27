using Alti.Core.Domain.Base;
using Alti.Core.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Alti.Core.Domain.Entities.Security;

/// Entidad usuario del sistema. Soporta clientes y staff (recepcionista/admin).

public class Usuario : EntidadBase
{
    public string Nombre { get; private set; } = string.Empty;
    public string Apellido { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string? Telefono { get; private set; }
    public string? Documento { get; private set; } 
    public EstadoUsuario Estado { get; private set; } = EstadoUsuario.Activo;
    public DateTime? UltimoAcceso { get; private set; }
    public int IntentosFallidos { get; private set; } = 0;
    public DateTime? BloqueadoHasta { get; private set; }

    public int RolId { get; private set; }
    public Rol Rol { get; private set; } = null!;

    public ICollection<Reserva> Reservas { get; private set; } = [];

    private Usuario() { }

    public static Usuario Crear(string nombre, string apellido, string email,
                                string passwordHash, int rolId, string? telefono = null,
                                string? documento = null)
    {
        return new Usuario
        {
            Nombre = nombre.Trim(),
            Apellido = apellido.Trim(),
            Email = email.Trim().ToLower(),
            PasswordHash = passwordHash,
            RolId = rolId,
            Telefono = telefono?.Trim(),
            Documento = documento?.Trim()
        };
    }

    public void ActualizarPerfil(string nombre, string apellido, string? telefono, string? documento)
    {
        Nombre = nombre.Trim();
        Apellido = apellido.Trim();
        Telefono = telefono?.Trim();
        Documento = documento?.Trim();
    }

    public void RegistrarAccesoExitoso()
    {
        UltimoAcceso = DateTime.UtcNow;
        IntentosFallidos = 0;
        BloqueadoHasta = null;
    }


    public void RegistrarAccesoFallido()
    {
        IntentosFallidos++;
        if (IntentosFallidos >= 5)
            BloqueadoHasta = DateTime.UtcNow.AddMinutes(30);
    }

    public bool EstaBloqueado() => BloqueadoHasta.HasValue && BloqueadoHasta > DateTime.UtcNow;
    public bool EstaActivo() => Estado == EstadoUsuario.Activo && !EstaEliminado;
    public string NombreCompleto() => $"{Nombre} {Apellido}";

    public void CambiarPassword(string nuevoHash) => PasswordHash = nuevoHash;
    public void CambiarEstado(EstadoUsuario estado) => Estado = estado;
    public void CambiarRol(int rolId) => RolId = rolId;
}