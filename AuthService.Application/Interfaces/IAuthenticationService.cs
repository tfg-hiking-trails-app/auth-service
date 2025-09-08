using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Update;

namespace AuthService.Application.Interfaces;

public interface IAuthenticationService
{
    Task<TokenResponseEntityDto> Login(AuthenticationEntityDto entityDto);

    Task<TokenResponseEntityDto> Refresh(string accessToken, string refreshToken);
    
    Task InvalidateRefreshToken(string token);

    Task EditPassword(Guid userCode, UpdatePasswordEntityDto updatePasswordEntityDto);

    bool AccessTokenBelongsToUser(string accessToken, string userName);

    bool AccessTokenBelongsToUser(string accessToken, Guid userCode);
}