using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using AutoMapper;

namespace AuthService.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenManager _tokenManager;
    private readonly IMapper _mapper;

    public AuthenticationService(
        IUserRepository userRepository, 
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        ITokenManager tokenManager,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _tokenManager = tokenManager;
        _mapper = mapper;
    }
    
    public async Task<TokenResponseEntityDto> Login(AuthenticationEntityDto entityDto)
    {
        User? user = await _userRepository.GetByUserName(entityDto.Username!);
        
        if (user is null || !_passwordHasher.VerifyHashedPassword(user.Password, entityDto.Password!))
            throw new UnauthorizedAccessException("Access Denied");

        UserEntityDto userEntityDto = _mapper.Map<UserEntityDto>(user);
        
        return new TokenResponseEntityDto()
        {
            AccessToken = _tokenManager.GenerateAccessToken(userEntityDto),
            RefreshToken = (await _tokenManager.GenerateRefreshToken(userEntityDto)).RefreshTokenValue
        };
    }

    public async Task<TokenResponseEntityDto> Refresh(string accessToken, string refreshToken)
    {
        RefreshToken? refreshTokenEntity = await _refreshTokenRepository.FindByRefreshTokenAsync(refreshToken);
        
        if (refreshTokenEntity is null || !refreshTokenEntity.Active || refreshTokenEntity.Expiration <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Access Denied");

        // The refresh token has already been used implying that it may have been stolen.
        if (refreshTokenEntity.Used)
        {
            await InvalidateAllUserTokens(refreshTokenEntity.User!);
            throw new UnauthorizedAccessException("Access Denied");
        }
        
        refreshTokenEntity.Used = true;

        User? user = await _userRepository.GetAsync(refreshTokenEntity.UserId);
        
        if (user is null || !AccessTokenBelongsToUser(accessToken, user.Username))
            throw new UnauthorizedAccessException("Access Denied");
        
        UserEntityDto userEntityDto = _mapper.Map<UserEntityDto>(user);
        string newAccessToken = _tokenManager.GenerateAccessToken(userEntityDto);
        
        RefreshTokenEntityDto refreshTokenEntityDto = await _tokenManager.GenerateRefreshToken(userEntityDto);
        
        return new TokenResponseEntityDto()
        {
            AccessToken = newAccessToken,
            RefreshToken = refreshTokenEntityDto.RefreshTokenValue
        };
    }

    public async Task InvalidateRefreshToken(string token)
    {
        RefreshToken? refreshToken = await _refreshTokenRepository.FindByRefreshTokenAsync(token);

        if (string.IsNullOrEmpty(refreshToken?.RefreshTokenValue))
            throw new NotFoundEntityException(nameof(RefreshToken), "refreshToken", token);

        refreshToken.Active = false;

        await _refreshTokenRepository.SaveChangesAsync();
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

    private bool AccessTokenBelongsToUser(string accessToken, string userName)
    {
        IDictionary<string, object> payload = _tokenManager.GetPayloadFromJwt(accessToken);
        
        return payload.ContainsKey("username") && payload["username"].Equals(userName);
    }
}