namespace AuthService.Application.DTOs;

public class TokenEntityDto
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}