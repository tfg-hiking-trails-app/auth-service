using AuthService.Application.DTOs;

namespace AuthService.Application.Interfaces;

public interface ITokenManager
{
    string GenerateAccessToken(UserEntityDto user);
    Task<RefreshTokenEntityDto> GenerateRefreshToken(UserEntityDto user);
    IDictionary<string, object> GetPayloadFromJwt(string token);
}