namespace AuthService.Application.DTOs.Create;

public class CreateRefreshTokenEntityDto
{
    public Guid Code { get; set; }
    public int UserId { get; set; }
    public string? RefreshTokenValue { get; set; }
    public bool Active { get; set; } = true;
    public DateTime Expiration { get; set; }
    public bool Used { get; set; } = false;
}