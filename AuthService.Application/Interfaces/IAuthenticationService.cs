using AuthService.Application.DTOs;

namespace AuthService.Application.Interfaces;

public interface IAuthenticationService
{
    Task<bool> Login(AuthenticationEntityDto entityDto);
}