using Alti.Domain.Entities;
using Core.domain.entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.ToTable("seasons");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id")
            .UseIdentityColumn();

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.StartDate)
            .HasColumnName("start_date")
            .IsRequired();

        builder.Property(s => s.EndDate)
            .HasColumnName("end_date")
            .IsRequired();

        builder.Property(s => s.Multiplier)
            .HasColumnName("multiplier")
            .HasColumnType("numeric(4,2)")
            .HasDefaultValue(1.00m)
            .IsRequired();

        builder.Property(s => s.Description)
            .HasColumnName("description");

        builder.Property(s => s.CreatedById)
            .HasColumnName("created_by_id")
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");
    }
}
