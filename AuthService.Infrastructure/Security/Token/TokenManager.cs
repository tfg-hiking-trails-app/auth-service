using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.CompilerServices;

namespace AuthService.Infrastructure.Security.Token;

public class TokenManager : ITokenManager
{
    private readonly IMapper _mapper;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public TokenManager(
        IMapper mapper,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _mapper = mapper;
        _refreshTokenRepository = refreshTokenRepository;
    }
    
    public string GenerateAccessToken(UserEntityDto user)
    {
        List<Claim> claims = new List<Claim>
        {
            new("username", user.Username!)
        };

        string? secretKey = Environment.GetEnvironmentVariable("ACCESS_TOKEN_SECRET_KEY");
        string? expiry = Environment.GetEnvironmentVariable("ACCESS_TOKEN_EXPIRE");
        string? issuer = Environment.GetEnvironmentVariable("ISSUER");
        string? audience = Environment.GetEnvironmentVariable("AUDIENCE");
        
        if (string.IsNullOrWhiteSpace(secretKey)  || string.IsNullOrWhiteSpace(expiry) || 
            string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
            throw new UnauthorizedAccessException("Access Token or expiry is empty");
        
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(IntegerType.FromString(expiry)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    public async Task<RefreshTokenEntityDto> GenerateRefreshToken(UserEntityDto user)
    {
        string? refreshTokenExpiration = Environment.GetEnvironmentVariable("REFRESH_TOKEN_EXPIRE");
        
        if (string.IsNullOrWhiteSpace(refreshTokenExpiration))
            throw new UnauthorizedAccessException("Refresh token expiration is empty");
        
        CreateRefreshTokenEntityDto createEntityDto = new CreateRefreshTokenEntityDto()
        {
            Expiration = DateTime.UtcNow.AddMinutes(IntegerType.FromString(refreshTokenExpiration)),
            RefreshTokenValue = Guid.NewGuid().ToString("N"),
            UserId = user.Id
        };

        RefreshToken refreshToken = _mapper.Map<RefreshToken>(createEntityDto);
        
        await _refreshTokenRepository.AddAsync(refreshToken);
        
        return _mapper.Map<RefreshTokenEntityDto>(refreshToken);
    }

    public IDictionary<string, object> GetPayloadFromJwt(string token)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        
        return handler.ReadJwtToken(token).Payload;
    }
}