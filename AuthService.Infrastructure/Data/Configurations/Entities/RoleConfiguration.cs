using AuthService.Domain.Entities;
using Common.Infrastructure.Data.Configuration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Data.Configurations.Entities;

public class RoleConfiguration : EntityConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("Role");
        builder.Property(e => e.RoleValue)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("role");
        builder.HasMany(d => d.Users)
            .WithOne(p => p.Role)
            .HasForeignKey(d => d.RoleId);
    }
}