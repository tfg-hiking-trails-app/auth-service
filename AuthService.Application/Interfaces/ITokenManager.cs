using AuthService.Application.DTOs;

namespace AuthService.Application.Interfaces;

public interface ITokenManager : Common.Application.Interfaces.ITokenManager
{
    string GenerateAccessToken(UserEntityDto user);
    Task<RefreshTokenEntityDto> GenerateRefreshToken(UserEntityDto user);
}