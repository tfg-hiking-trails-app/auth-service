using AuthService.Domain.Entities;
using Common.Infrastructure.Data.Configuration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Data.Configurations.Entities;

public class UserConfiguration : EntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("User");
        
        builder.HasOne(d => d.Role)
            .WithMany(p => p.Users)
            .HasForeignKey(d => d.RoleId);
        
        builder.HasOne(d => d.Status)
            .WithMany(p => p.Users)
            .HasForeignKey(d => d.StatusId);
        
        builder.Property(e => e.Username)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("username");
        
        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("email");
        
        builder.Property(e => e.Password)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("password");
        
        builder.Property(e => e.LastLogin)
            .HasColumnType("timestamp")
            .HasColumnName("last_login");
    }
}