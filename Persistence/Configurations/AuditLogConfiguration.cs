using Alti.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("id")
            .UseIdentityColumn();

        builder.Property(a => a.UserId)
            .HasColumnName("user_id");

        builder.Property(a => a.ExecutorRole)
            .HasColumnName("executor_role")
            .HasConversion<string>();

        builder.Property(a => a.Action)
            .HasColumnName("action")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(a => a.Entity)
            .HasColumnName("entity")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(a => a.EntityId)
            .HasColumnName("entity_id");

        builder.Property(a => a.PreviousData)
            .HasColumnName("previous_data")
            .HasColumnType("jsonb");

        builder.Property(a => a.NewData)
            .HasColumnName("new_data")
            .HasColumnType("jsonb");

        builder.Property(a => a.IpAddress)
            .HasColumnName("ip_address")
            .HasMaxLength(45);

        builder.Property(a => a.Description)
            .HasColumnName("description");

        builder.Property(a => a.ExecutedAt)
            .HasColumnName("executed_at")
            .HasDefaultValueSql("NOW()");

        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => new { a.Entity, a.EntityId });
        builder.HasIndex(a => a.ExecutedAt);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}