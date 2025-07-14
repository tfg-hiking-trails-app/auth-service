namespace AuthService.Application.DTOs;

public class TokenEntityDto
{
    public TokenEntityDto SetAccessToken(string accessToken)
    {
        AccessToken = accessToken;
        return this;
    }

    public TokenEntityDto SetRefreshToken(string refreshToken)
    {
        RefreshToken = refreshToken;
        return this;
    }
    
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}