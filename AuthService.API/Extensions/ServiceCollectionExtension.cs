using AuthService.API.DTOs.Mapping;
using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Data.Configurations.Mapping;
using AuthService.Infrastructure.Data.Repositories;
using AuthService.Infrastructure.Security;
using Microsoft.OpenApi.Models;

namespace AuthService.API.Extensions;

public static class ServiceCollectionExtension
{
    public static void ConfigureServicesCollection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRouting(options => options.LowercaseUrls = true);
        
        services.AddDbContext<AuthServiceDbContext>();

        services.AddAutoMapper();
        
        services.AddServices(configuration);
        
        services.AddRepositories(configuration);
        
        services.AddSwaggerGen();
    }

    private static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
    }
    
    private static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
    }

    private static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(CommonProfile).Assembly,
            typeof(AuthenticationProfile).Assembly,
            typeof(UserProfile).Assembly,
            typeof(RoleProfile).Assembly,
            typeof(StatusProfile).Assembly,
            typeof(CommonEntityProfile).Assembly,
            typeof(UserEntityProfile).Assembly,
            typeof(RoleEntityProfile).Assembly,
            typeof(StatusEntityProfile).Assembly
        );
    }

    private static void AddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Authentication and user creation microservice", 
                Version = "3.0.1"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });
    }
    
}