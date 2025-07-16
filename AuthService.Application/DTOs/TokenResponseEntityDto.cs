namespace AuthService.Application.DTOs;

public class TokenResponseEntityDto
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}