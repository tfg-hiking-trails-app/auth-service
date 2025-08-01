using AuthService.Domain.Entities;
using Common.Infrastructure.Data.Configuration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Data.Configurations.Entities;

public class RefreshTokenConfiguration : EntityConfiguration<RefreshToken>
{
    public override void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        base.Configure(builder);

        builder.ToTable("RefreshToken");
        
        builder.HasOne(d => d.User)
            .WithMany(p => p.RefreshTokens)
            .HasForeignKey(d => d.UserId);
        
        builder.Property(e => e.RefreshTokenValue)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("refresh_token");
        
        builder.Property(e => e.Active)
            .IsRequired()
            .HasColumnName("active");
        
        builder.Property(e => e.Expiration)
            .IsRequired()
            .HasColumnName("expiration");
        
        builder.Property(e => e.Used)
            .IsRequired()
            .HasColumnName("used");
    }
}