using Alti.Core.Domain.Entities.Booking;
using Alti.Core.Domain.Entities.Hotel;
using Alti.Core.Domain.Entities.Security;
using Alti.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Alti.Infrastructure.Data.Configurations;

// ─── Rol ──────
internal sealed class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> b)
    {
        b.ToTable("roles");
        b.HasKey(r => r.Id);
        b.Property(r => r.Nombre).HasMaxLength(50).IsRequired();
        b.Property(r => r.Descripcion).HasMaxLength(200);
        b.Property(r => r.Tipo).HasConversion<int>().IsRequired();
        b.HasIndex(r => r.Nombre).IsUnique();
        b.Property(r => r.RowVersion).IsRowVersion();

        b.HasData(
            new
            {
                Id = 1,
                Nombre = "Cliente",
                Tipo = TipoRol.Cliente,
                Descripcion = "Usuario cliente del portal web",
                CreadoEn = DateTime.UtcNow,
                EstaEliminado = false,
                RowVersion = Array.Empty<byte>()
            },
            new
            {
                Id = 2,
                Nombre = "Recepcionista",
                Tipo = TipoRol.Recepcionista,
                Descripcion = "Staff del hotel — app desktop",
                CreadoEn = DateTime.UtcNow,
                EstaEliminado = false,
                RowVersion = Array.Empty<byte>()
            },
            new
            {
                Id = 3,
                Nombre = "Administrador",
                Tipo = TipoRol.Administrador,
                Descripcion = "Acceso total al sistema",
                CreadoEn = DateTime.UtcNow,
                EstaEliminado = false,
                RowVersion = Array.Empty<byte>()
            }
        );
    }
}

// ─── Usuario ───-
internal sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> b)
    {
        b.ToTable("usuarios");
        b.HasKey(u => u.Id);
        b.Property(u => u.Nombre).HasMaxLength(100).IsRequired();
        b.Property(u => u.Apellido).HasMaxLength(100).IsRequired();
        b.Property(u => u.Email).HasMaxLength(255).IsRequired();
        b.Property(u => u.PasswordHash).HasMaxLength(500).IsRequired();
        b.Property(u => u.Telefono).HasMaxLength(20);
        b.Property(u => u.Documento).HasMaxLength(30);
        b.Property(u => u.Estado).HasConversion<int>().IsRequired();
        b.Property(u => u.RowVersion).IsRowVersion();

        b.HasIndex(u => u.Email).IsUnique();
        b.HasIndex(u => u.Documento);

        b.HasOne(u => u.Rol)
         .WithMany(r => r.Usuarios)
         .HasForeignKey(u => u.RolId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}

// ─── Categoria ────
internal sealed class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> b)
    {
        b.ToTable("categorias");
        b.HasKey(c => c.Id);
        b.Property(c => c.Nombre).HasMaxLength(100).IsRequired();
        b.Property(c => c.Descripcion).HasMaxLength(500);
        b.Property(c => c.PrecioBaseNoche).HasPrecision(10, 2).IsRequired();
        b.Property(c => c.MultiplierTemporadaAlta).HasPrecision(4, 2);
        b.Property(c => c.ImagenUrl).HasMaxLength(500);
        b.Property(c => c.RowVersion).IsRowVersion();
    }
}

// ─── Habitacion ────
internal sealed class HabitacionConfiguration : IEntityTypeConfiguration<Habitacion>
{
    public void Configure(EntityTypeBuilder<Habitacion> b)
    {
        b.ToTable("habitaciones");
        b.HasKey(h => h.Id);
        b.Property(h => h.Numero).HasMaxLength(10).IsRequired();
        b.Property(h => h.Estado).HasConversion<int>().IsRequired();
        b.Property(h => h.Notas).HasMaxLength(500);
        b.Property(h => h.RowVersion).IsRowVersion();

        b.HasIndex(h => h.Numero).IsUnique();

        b.HasOne(h => h.Categoria)
         .WithMany(c => c.Habitaciones)
         .HasForeignKey(h => h.CategoriaId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}

// ─── Reserva ─────
internal sealed class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
{
    public void Configure(EntityTypeBuilder<Reserva> b)
    {
        b.ToTable("reservas");
        b.HasKey(r => r.Id);
        b.Property(r => r.CodigoConfirmacion).HasMaxLength(30).IsRequired();
        b.Property(r => r.Estado).HasConversion<int>().IsRequired();
        b.Property(r => r.TotalCalculado).HasPrecision(10, 2).IsRequired();
        b.Property(r => r.NotasEspeciales).HasMaxLength(500);
        b.Property(r => r.MotivosCancelacion).HasMaxLength(500);
        b.Property(r => r.RowVersion).IsRowVersion();

        b.HasIndex(r => r.CodigoConfirmacion).IsUnique();
        b.HasIndex(r => new { r.HabitacionId, r.FechaEntrada, r.FechaSalida })
         .HasDatabaseName("IX_reservas_habitacion_fechas");

        b.HasOne(r => r.Usuario)
         .WithMany(u => u.Reservas)
         .HasForeignKey(r => r.UsuarioId)
         .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(r => r.Habitacion)
         .WithMany(h => h.Reservas)
         .HasForeignKey(r => r.HabitacionId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}

// ─── Pago ───────
internal sealed class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> b)
    {
        b.ToTable("pagos");
        b.HasKey(p => p.Id);
        b.Property(p => p.ReferenciaPago).HasMaxLength(30).IsRequired();
        b.Property(p => p.Monto).HasPrecision(10, 2).IsRequired();
        b.Property(p => p.MetodoPago).HasConversion<int>().IsRequired();
        b.Property(p => p.Estado).HasConversion<int>().IsRequired();
        b.Property(p => p.ReferenciaExterna).HasMaxLength(200);
        b.Property(p => p.MotivoFallo).HasMaxLength(500);
        b.Property(p => p.MotivoReembolso).HasMaxLength(500);
        b.Property(p => p.RowVersion).IsRowVersion();

        b.HasIndex(p => p.ReferenciaPago).IsUnique();

        b.HasOne(p => p.Reserva)
         .WithMany(r => r.Pagos)
         .HasForeignKey(p => p.ReservaId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}