namespace AuthService.Application.DTOs;

public record AuthenticationEntityDto
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}