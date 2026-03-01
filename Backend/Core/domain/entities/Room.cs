using Alti.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.domain.entities
{
    internal class Room
    {
        public string Numero { get; private set; } = string.Empty;
        public TipoHabitacion Tipo { get; private set; }
        public short Piso { get; private set; }
        public short Capacidad { get; private set; }
        public decimal Precio_Base { get; private set; }
        public string? Descripcion { get; private set; }
        public EstadoHabitacion Estado { get; private set; } = EstadoHabitacion.Disponible;
        public int Row_Version { get; private set; } = 0;
    }
}
