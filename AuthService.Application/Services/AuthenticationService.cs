using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AutoMapper;

namespace AuthService.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthenticationService(
        IUserRepository userRepository, 
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _mapper = mapper;
    }
    
    public async Task<TokenEntityDto> Login(AuthenticationEntityDto entityDto)
    {
        User? user = await _userRepository.GetByUserName(entityDto.Username!);
        
        if (user is null || !_passwordHasher.VerifyHashedPassword(user.Password, entityDto.Password!))
            throw new UnauthorizedAccessException("Access Denied");

        UserEntityDto userEntityDto = _mapper.Map<UserEntityDto>(user);
        
        return new TokenEntityDto()
        {
            AccessToken = _tokenService.GenerateAccessToken(userEntityDto),
            RefreshToken = (await _tokenService.GenerateRefreshToken(userEntityDto)).RefreshTokenValue
        };
    }

    public async Task<TokenEntityDto> Refresh(TokenEntityDto tokenDto)
    {
        if (tokenDto.RefreshToken is null)
            throw new ArgumentNullException($"RefreshToken");
        
        RefreshToken? refreshToken = await _refreshTokenRepository
            .FindByRefreshTokenAsync(tokenDto.RefreshToken!);
        
        if (refreshToken is null || !refreshToken.Active || refreshToken.Expiration <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Access Denied");

        // The refresh token has already been used implying that it may have been stolen.
        if (refreshToken.Used)
        {
            await InvalidateAllUserTokens(refreshToken.User!);
            throw new UnauthorizedAccessException("Access Denied");
        }
        
        refreshToken.Used = true;

        User? user = await _userRepository.GetAsync(refreshToken.UserId);
        
        if (user is null)
            throw new UnauthorizedAccessException("Access Denied");
        
        UserEntityDto userEntityDto = _mapper.Map<UserEntityDto>(user);
        string newAccessToken = _tokenService.GenerateAccessToken(userEntityDto);
        
        RefreshTokenEntityDto refreshTokenEntityDto = await _tokenService.GenerateRefreshToken(userEntityDto);
        
        return new TokenEntityDto()
        {
            AccessToken = newAccessToken,
            RefreshToken = refreshTokenEntityDto.RefreshTokenValue
        };
    }

    private async Task InvalidateAllUserTokens(User user)
    {
        IEnumerable<RefreshToken> refreshTokens = await _refreshTokenRepository
            .GetAllValidRefreshTokensByUserAsync(user.Code);

        foreach (RefreshToken refreshToken in refreshTokens)
        {
            refreshToken.Used = true;
            refreshToken.Active = false;
        }

        await _refreshTokenRepository.SaveChangesAsync();
    }
}