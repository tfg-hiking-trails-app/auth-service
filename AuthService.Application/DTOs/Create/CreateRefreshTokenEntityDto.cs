namespace AuthService.Application.DTOs.Create;

public class CreateRefreshTokenEntityDto
{
    public int UserId { get; set; }
    public string? RefreshTokenValue { get; set; }
    public bool Active { get; set; } = true;
    public DateTime Expiration { get; set; }
    public bool Used { get; set; } = false;
}