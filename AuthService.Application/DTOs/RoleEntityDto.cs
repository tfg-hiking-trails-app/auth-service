namespace AuthService.Application.DTOs;

public record RoleEntityDto
{
    public Guid Code { get; set; }
    public string? RoleValue { get; set; }
}