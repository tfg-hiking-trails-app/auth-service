﻿using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthService.Infrastructure.Data
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
            string defaultConnection = GetDefaultConnectionToDatabase();
            
            optionsBuilder
                .UseMySql(
                    defaultConnection,
                    ServerVersion.AutoDetect(defaultConnection)
                )
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Searches for entities that implement an entity configuration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private string GetDefaultConnectionToDatabase()
        {
            string server = Environment.GetEnvironmentVariable("MARIADB_SERVER") ?? "";
            string database = Environment.GetEnvironmentVariable("MARIADB_DATABASE") ?? "";
            string user = Environment.GetEnvironmentVariable("MARIADB_USER") ?? "";
            string password = Environment.GetEnvironmentVariable("MYSQL_ROOT_PASSWORD") ?? "";
            
            return $"Server={server};Database={database};User={user};Password={password};TreatTinyAsBoolean=true;";
        }
    }
}
