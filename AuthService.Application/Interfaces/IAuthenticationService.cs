using AuthService.Application.DTOs;

namespace AuthService.Application.Interfaces;

public interface IAuthenticationService
{
    Task<TokenEntityDto> Login(AuthenticationEntityDto entityDto);
}