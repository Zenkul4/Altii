using FluentValidation;
using System;


// Validadores de lógica de negocio usando FluentValidation.
// Cada validador está en su propia clase para máxima mantenibilidad.


namespace Alti.Application.Validators;

public sealed record RegistrarUsuarioDto(
    string Nombre, string Apellido, string Email,
    string Password, string? Telefono, string? Documento, int RolId);

public sealed record CrearReservaDto(
    int UsuarioId, int HabitacionId, DateTime FechaEntrada,
    DateTime FechaSalida, int NumeroHuespedes, string? NotasEspeciales,
    bool PagoRequerido = true);

public sealed record CrearPagoDto(
    int ReservaId, decimal Monto,
    Alti.Core.Domain.Enums.MetodoPago MetodoPago, string? ReferenciaExterna);

public sealed record CrearHabitacionDto(
    string Numero, int Piso, int CategoriaId, string? Notas);

public sealed record CrearCategoriaDto(
    string Nombre, decimal PrecioBaseNoche, int CapacidadMaxima,
    string? Descripcion, string? ImagenUrl, decimal? MultiplierTemporadaAlta);


public sealed class ValidadorRegistrarUsuario : AbstractValidator<RegistrarUsuarioDto>
{
    public ValidadorRegistrarUsuario()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(100);

        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es requerido.")
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress().WithMessage("El email no tiene formato válido.")
            .MaximumLength(255);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .Matches(@"[A-Z]").WithMessage("Debe contener al menos una letra mayúscula.")
            .Matches(@"[0-9]").WithMessage("Debe contener al menos un número.");

        RuleFor(x => x.Telefono)
            .MaximumLength(20)
            .Matches(@"^\+?[\d\s\-\(\)]+$").WithMessage("Formato de teléfono inválido.")
            .When(x => !string.IsNullOrEmpty(x.Telefono));

        RuleFor(x => x.Documento)
            .MaximumLength(30)
            .When(x => !string.IsNullOrEmpty(x.Documento));

        RuleFor(x => x.RolId)
            .GreaterThan(0).WithMessage("Debe especificarse un rol válido.");
    }
}


/// Valida la creación de reservas.
public sealed class ValidadorCrearReserva : AbstractValidator<CrearReservaDto>
{
    public ValidadorCrearReserva()
    {
        RuleFor(x => x.UsuarioId)
            .GreaterThan(0).WithMessage("Usuario requerido.");

        RuleFor(x => x.HabitacionId)
            .GreaterThan(0).WithMessage("Habitación requerida.");

        RuleFor(x => x.FechaEntrada)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("La fecha de entrada no puede ser en el pasado.");

        RuleFor(x => x.FechaSalida)
            .GreaterThan(x => x.FechaEntrada)
            .WithMessage("La fecha de salida debe ser posterior a la de entrada.");


        RuleFor(x => x)
            .Must(x => (x.FechaSalida - x.FechaEntrada).Days <= 30)
            .WithMessage("Una reserva no puede exceder 30 noches.")
            .WithName("FechasReserva");

        RuleFor(x => x)
            .Must(x => (x.FechaSalida - x.FechaEntrada).Days >= 1)
            .WithMessage("La estancia mínima es de 1 noche.")
            .WithName("FechasReserva");

        RuleFor(x => x.NumeroHuespedes)
            .InclusiveBetween(1, 10)
            .WithMessage("El número de huéspedes debe ser entre 1 y 10.");

        RuleFor(x => x.NotasEspeciales)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.NotasEspeciales));
    }
}


public sealed class ValidadorCrearPago : AbstractValidator<CrearPagoDto>
{
    public ValidadorCrearPago()
    {
        RuleFor(x => x.ReservaId)
            .GreaterThan(0).WithMessage("Reserva requerida.");

        RuleFor(x => x.Monto)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a cero.")
            .LessThanOrEqualTo(99_999.99m).WithMessage("Monto excede el límite permitido.");

        RuleFor(x => x.MetodoPago)
            .IsInEnum().WithMessage("Método de pago inválido.");

        RuleFor(x => x.ReferenciaExterna)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.ReferenciaExterna));
    }
}


public sealed class ValidadorCrearHabitacion : AbstractValidator<CrearHabitacionDto>
{
    public ValidadorCrearHabitacion()
    {
        RuleFor(x => x.Numero)
            .NotEmpty()
            .MaximumLength(10)
            .Matches(@"^[A-Za-z0-9\-]+$").WithMessage("El número solo puede contener letras, números y guiones.");

        RuleFor(x => x.Piso)
            .InclusiveBetween(-2, 100).WithMessage("Piso inválido.");

        RuleFor(x => x.CategoriaId)
            .GreaterThan(0).WithMessage("Categoría requerida.");

        RuleFor(x => x.Notas)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Notas));
    }
}


public sealed class ValidadorCrearCategoria : AbstractValidator<CrearCategoriaDto>
{
    public ValidadorCrearCategoria()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.PrecioBaseNoche)
            .GreaterThan(0).WithMessage("El precio base debe ser mayor a cero.");

        RuleFor(x => x.CapacidadMaxima)
            .InclusiveBetween(1, 20).WithMessage("Capacidad debe estar entre 1 y 20 personas.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Descripcion));

        RuleFor(x => x.ImagenUrl)
            .MaximumLength(500)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("La URL de imagen no es válida.")
            .When(x => !string.IsNullOrEmpty(x.ImagenUrl));

        RuleFor(x => x.MultiplierTemporadaAlta)
            .InclusiveBetween(1.0m, 5.0m).WithMessage("El multiplicador debe estar entre 1.0 y 5.0.")
            .When(x => x.MultiplierTemporadaAlta.HasValue);
    }
}