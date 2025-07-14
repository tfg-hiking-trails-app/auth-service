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
        
        string? secretKey = _env.IsDevelopment() 
            ? _configuration["AccessTokenJwt:SecretKey"] 
            : Environment.GetEnvironmentVariable("ACCESS_TOKEN_SECRET_KEY");
        string? expiry = _env.IsDevelopment()
            ? _configuration["AccessTokenJwt:Expiry"]
            : Environment.GetEnvironmentVariable("ACCESS_TOKEN_EXPIRY");
        
        if (string.IsNullOrWhiteSpace(secretKey)  || string.IsNullOrWhiteSpace(expiry))
            throw new UnauthorizedAccessException("Access Token or expiry is empty");
        
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        JwtSecurityToken token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(IntegerType.FromString(expiry)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}