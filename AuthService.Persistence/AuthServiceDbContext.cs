using AuthService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AuthService.Persistence
{
    public class AuthServiceDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AuthServiceDbContext(DbContextOptions<AuthServiceDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                _configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(12, 0, 1))
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");
                entity.Property(e => e.RoleValue)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("role");
                entity.HasMany(d => d.Users)
                    .WithOne(p => p.Role)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");
                entity.Property(e => e.StatusValue)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("status");
                entity.HasMany(d => d.Users)
                    .WithOne(p => p.Status)
                    .HasForeignKey(d => d.StatusId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId);
                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.StatusId);
                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("username");
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("email");
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("password");
                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("first_name");
                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("last_name");
                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("date_of_birth");
                entity.Property(e => e.LastLogin)
                    .HasColumnType("timestamp")
                    .HasColumnName("last_login");
                entity.Property(e => e.ProfilePictureUrl)
                    .HasMaxLength(255)
                    .HasColumnName("profile_picture_url");
            });
        }
    }
}
