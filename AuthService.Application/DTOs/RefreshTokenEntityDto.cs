namespace AuthService.Application.DTOs;

public record RefreshTokenEntityDto
{
    public int UserId { get; set; }
    public string? RefreshTokenValue { get; set; }
    public bool Active { get; set; }
    public DateTime? Expiration { get; set; }
    public bool Used { get; set; }
}