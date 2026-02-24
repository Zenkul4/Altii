namespace Alti.Core.Domain.Enums;

public enum EstadoReserva
{
    Pendiente = 1,
    Confirmada = 2,
    CheckIn = 3,
    CheckOut = 4,
    Cancelada = 5,
    NoShow = 6
}

public enum EstadoHabitacion
{
    Disponible = 1,
    Ocupada = 2,
    EnLimpieza = 3,
    Mantenimiento = 4,
    FueraDeServicio = 5
}

public enum EstadoPago
{
    Pendiente = 1,
    Procesando = 2,
    Completado = 3,
    Fallido = 4,
    Reembolsado = 5
}

public enum MetodoPago
{
    Efectivo = 1,
    TarjetaCredito = 2,
    TarjetaDebito = 3,
    Transferencia = 4,
    PagoEnLinea = 5
}

public enum TipoRol
{
    Cliente = 1,
    Recepcionista = 2,
    Administrador = 3
}

public enum EstadoUsuario
{
    Activo = 1,
    Inactivo = 2,
    Suspendido = 3
}