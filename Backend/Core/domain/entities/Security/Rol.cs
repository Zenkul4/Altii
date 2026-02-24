using Alti.Core.Domain.Base;
using Alti.Core.Domain.Enums;
using System.Collections.Generic;

namespace Alti.Core.Domain.Entities.Security;

/// Define los roles del sistema y sus permisos asociados.
public class Rol : EntidadBase
{
    public string Nombre { get; private set; } = string.Empty;
    public string? Descripcion { get; private set; }
    public TipoRol Tipo { get; private set; }

    public ICollection<Usuario> Usuarios { get; private set; } = [];

    private Rol() { }

    public static Rol Crear(string nombre, TipoRol tipo, string? descripcion = null)
    {
        return new Rol
        {
            Nombre = nombre.Trim(),
            Tipo = tipo,
            Descripcion = descripcion?.Trim()
        };
    }

    public void Actualizar(string nombre, string? descripcion)
    {
        Nombre = nombre.Trim();
        Descripcion = descripcion?.Trim();
    }
}