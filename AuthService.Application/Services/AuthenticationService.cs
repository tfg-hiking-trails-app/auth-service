using System.Text;
using System.Text.Json;
using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Update;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AutoMapper;
using Common.Domain.Exceptions;
using Common.Domain.Interfaces.Messaging;

namespace AuthService.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenManager _tokenManager;
    private readonly IRabbitMqQueueProducer _rabbitMqQueueProducer;
    private readonly IMapper _mapper;

    public AuthenticationService(
        IUserRepository userRepository, 
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        ITokenManager tokenManager,
        IRabbitMqQueueProducer rabbitMqQueueProducer,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _tokenManager = tokenManager;
        _rabbitMqQueueProducer = rabbitMqQueueProducer;
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

    public async Task EditPassword(Guid userCode, UpdatePasswordEntityDto updatePasswordEntityDto)
    {
        User? user = await _userRepository.GetByCodeAsync(userCode);
        
        if (user is null)
            throw new NotFoundEntityException(nameof(User), userCode);
        
        // Check old password
        if (!_passwordHasher.VerifyHashedPassword(user.Password, updatePasswordEntityDto.Password!))
            throw new ArgumentException("Password from user is incorrect");
        
        // Save new password
        user.Password = _passwordHasher.HashPassword(updatePasswordEntityDto.NewPassword);

        await _userRepository.SaveChangesAsync();
    }

    public async Task EditUsername(Guid userCode, string newUsername)
    {
        User? user = await _userRepository.GetByCodeAsync(userCode);
        
        if (user is null)
            throw new NotFoundEntityException(nameof(User), userCode);
        
        // The new username is the same as the current username.
        if (user.Username.Equals(newUsername))
            throw new ArgumentException("This is your username");
        
        // Check if exists new username
        if (await _userRepository.GetByUserName(newUsername) is not null)
            throw new ArgumentException("Username already exists");
        
        await PublishUsernameChangeAsync(user.Username, newUsername);
        
        user.Username = newUsername;
        
        await _userRepository.SaveChangesAsync();

    }

    private async Task PublishUsernameChangeAsync(string oldUsername, string newUsername)
    {
        string body = JsonSerializer.Serialize(new UpdateUsernameEntityDto
        {
            OldUsername = oldUsername,
            NewUsername = newUsername
        });

        await _rabbitMqQueueProducer.BasicPublishAsync(Encoding.UTF8.GetBytes(body));
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

    public bool AccessTokenBelongsToUser(string accessToken, string userName)
    {
        IDictionary<string, object> payload = _tokenManager.GetPayloadFromJwt(accessToken);
        
        return payload.ContainsKey("username") && payload["username"].Equals(userName);
    }
    
    public bool AccessTokenBelongsToUser(string accessToken, Guid userCode)
    {
        IDictionary<string, object> payload = _tokenManager.GetPayloadFromJwt(accessToken);
        
        return payload.ContainsKey("userCode") && payload["userCode"].Equals(userCode.ToString());
    }
    
}