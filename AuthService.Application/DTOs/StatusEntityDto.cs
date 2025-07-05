namespace AuthService.Application.DTOs;

public record StatusEntityDto
{
    public Guid Code { get; set; }
    public string? StatusValue { get; set; }
}