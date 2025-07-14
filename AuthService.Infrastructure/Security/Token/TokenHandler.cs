using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.CompilerServices;

namespace AuthService.Infrastructure.Security.Token;

public class TokenHandler : ITokenHandler
{
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _configuration;

    public TokenHandler(IWebHostEnvironment env, IConfiguration configuration)
    {
        _env = env;
        _configuration = configuration;
    }
    
    public string GenerateAccessToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new("username", user.Username)
        };

        string? secretKey = GetProperty("AccessTokenJwt:SecretKey", "ACCESS_TOKEN_SECRET_KEY");
        string? expiry = GetProperty("AccessTokenJwt:Expiry","ACCESS_TOKEN_EXPIRY");
        
        if (string.IsNullOrWhiteSpace(secretKey)  || string.IsNullOrWhiteSpace(expiry))
            throw new UnauthorizedAccessException("Access Token or expiry is empty");
        
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: GetProperty("Jwt:Issuer", "ISSUER"),
            audience: GetProperty("Jwt:Audience", "AUDIENCE"),
            claims: claims,
            expires: DateTime.Now.AddMinutes(IntegerType.FromString(expiry)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GetProperty(string configurationKey, string propertyName)
    {
        string? secretKey = _env.IsDevelopment() 
            ? _configuration[configurationKey] 
            : Environment.GetEnvironmentVariable(propertyName);
        
        if (string.IsNullOrWhiteSpace(secretKey))
            throw new UnauthorizedAccessException($"{propertyName} is empty");

        return secretKey;
    }
    
}