using AuthService.Application.DTOs;

namespace AuthService.Application.Interfaces;

public interface IAuthenticationService
{
    Task<TokenResponseEntityDto> Login(AuthenticationEntityDto entityDto);

    Task<TokenResponseEntityDto> Refresh(TokenResponseEntityDto tokenResponseDto);
}