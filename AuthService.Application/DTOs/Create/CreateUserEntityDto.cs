using Common.Application.DTOs.Create;

namespace AuthService.Application.DTOs.Create;

public record CreateUserEntityDto : CreateBaseEntityDto
{
    public Guid RoleCode { get; set; }
    public Guid StatusCode { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
    public DateTime? LastLogin { get; set; }
}