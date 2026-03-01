using Alti.Core.Domain.Entities.Booking;
using Alti.Core.Domain.Entities.Hotel;
using Alti.Core.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alti.Infrastructure.Data.Configurations;

public class HabitacionConfiguration : IEntityTypeConfiguration<Habitacion>
{
    public void Configure(EntityTypeBuilder<Habitacion> builder)
    {
        builder.ToTable("Habitaciones");
        builder.HasKey(h => h.Id);

        builder.Property(h => h.Numero).IsRequired().HasMaxLength(10);
        builder.HasIndex(h => h.Numero).IsUnique();

        builder.Property(h => h.RowVersion).IsRowVersion();

        builder.HasOne(h => h.Categoria)
               .WithMany(c => c.Habitaciones)
               .HasForeignKey(h => h.CategoriaId)
               .OnDelete(DeleteBehavior.Restrict); 
    }
}

public class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
{
    public void Configure(EntityTypeBuilder<Reserva> builder)
    {
        builder.ToTable("Reservas");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.CodigoConfirmacion).IsRequired().HasMaxLength(50);
        builder.HasIndex(r => r.CodigoConfirmacion).IsUnique();

        builder.Property(r => r.TotalCalculado).HasColumnType("decimal(18,2)");

        builder.Property(r => r.RowVersion).IsRowVersion();

        builder.HasOne(r => r.Usuario)
               .WithMany(u => u.Reservas)
               .HasForeignKey(r => r.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Habitacion)
               .WithMany(h => h.Reservas)
               .HasForeignKey(r => r.HabitacionId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

public class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> builder)
    {
        builder.ToTable("Pagos");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Monto).HasColumnType("decimal(18,2)");
        builder.Property(p => p.TransaccionExternaId).HasMaxLength(100);

        builder.HasOne(p => p.Reserva)
               .WithMany(r => r.Pagos)
               .HasForeignKey(p => p.ReservaId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}