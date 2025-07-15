using AuthService.Application.DTOs;

namespace AuthService.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(UserEntityDto user);
    Task<RefreshTokenEntityDto> GenerateRefreshToken(UserEntityDto user);
}