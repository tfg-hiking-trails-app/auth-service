using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;

namespace AuthService.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthenticationService(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }
    
    public async Task<TokenEntityDto> Login(AuthenticationEntityDto entityDto)
    {
        User? user = await _userRepository.GetByUserName(entityDto.Username!);
        
        if (user == null || !_passwordHasher.VerifyHashedPassword(user.Password, entityDto.Password!))
            throw new UnauthorizedAccessException("Access Denied");

        return new TokenEntityDto()
        {
            AccessToken = _tokenService.GenerateAccessToken(user),
            RefreshToken = _tokenService.GenerateRefreshToken(user)
        };
    }
}