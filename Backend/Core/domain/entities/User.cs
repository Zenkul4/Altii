using System;
using System.Collections.Generic;
using System.Text;

namespace Core.domain.entities
{
    public class User
    {
        public string Nombre { get; private set; } = string.Empty;
        public string Apellido { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Password_Hash { get; private set; } = string.Empty;
        public string? Telefono { get; private set; }
        public RolUsuario Rol { get; private set; } = RolUsuario.Huesped;
        public bool Activo { get; private set; } = true;

    }
}
