using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;

namespace AuthService.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenHandler _tokenHandler;

    public AuthenticationService(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher,
        ITokenHandler tokenHandler)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenHandler = tokenHandler;
    }
    
    public async Task<TokenEntityDto> Login(AuthenticationEntityDto entityDto)
    {
        User? user = await _userRepository.GetByUserNameAsync(entityDto.Username!);
        
        if (user == null || !_passwordHasher.VerifyHashedPassword(user.Password, entityDto.Password!))
            throw new UnauthorizedAccessException("Access Denied");
        
        return new TokenEntityDto()
            .SetAccessToken(_tokenHandler.GenerateAccessToken(user));
    }
}