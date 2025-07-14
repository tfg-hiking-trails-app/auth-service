using System.Text;
using AuthService.API.DTOs.Mapping;
using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Data.Configurations.Mapping;
using AuthService.Infrastructure.Data.Repositories;
using AuthService.Infrastructure.Security.Encryption;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TokenHandler = AuthService.Infrastructure.Security.Token.TokenHandler;

namespace AuthService.API.Extensions;

public static class ServiceCollectionExtension
{
    public static void ConfigureServicesCollection(
        this IServiceCollection services, 
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddRouting(options => options.LowercaseUrls = true);
        
        services.AddDbContext<AuthServiceDbContext>();

        services.AddAutoMapper();
        
        services.AddServices();
        
        services.AddRepositories();
        
        services.AddJwtBearer(configuration, environment);
        
        services.AddSwaggerGen();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenHandler, TokenHandler>();
    }
    
    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
    }

    private static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(CommonProfile).Assembly,
            typeof(TokenProfile).Assembly,
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

    private static void AddJwtBearer(
        this IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment environment)
    {
        string? secretKey = environment.IsDevelopment() 
            ? configuration["AccessTokenJwt:SecretKey"] 
            : Environment.GetEnvironmentVariable("ACCESS_TOKEN_SECRET_KEY");
        string? issuer = environment.IsDevelopment()
            ? configuration["Jwt:Issuer"]
            : Environment.GetEnvironmentVariable("ISSUER");
        string? audience = environment.IsDevelopment()
            ? configuration["Jwt:Audience"]
            : Environment.GetEnvironmentVariable("AUDIENCE");
        
        if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            throw new Exception("Access token not found");
        
        services
            .AddHttpContextAccessor()
            .AddAuthorization()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
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