using AuthService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Data.Configurations.Entities;

public class StatusConfiguration : EntityConfiguration<Status>
{
    public override void Configure(EntityTypeBuilder<Status> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("Status");
        builder.Property(e => e.StatusValue)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("status");
        builder.HasMany(d => d.Users)
            .WithOne(p => p.Status)
            .HasForeignKey(d => d.StatusId);
    }
}