using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;

namespace AuthService.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AuthenticationService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<bool> Login(AuthenticationEntityDto entityDto)
    {
        User? user = await _userRepository.GetByUserNameAsync(entityDto.Username!);
        
        if (user == null)
            throw new NotFoundEntityException($"The user '{entityDto.Username}' does not exists");
        
        if (!_passwordHasher.VerifyHashedPassword(user.Password, entityDto.Password!))
            throw new UnauthorizedAccessException("Wrong password");

        // Devolver un token
        return true;
    }
}