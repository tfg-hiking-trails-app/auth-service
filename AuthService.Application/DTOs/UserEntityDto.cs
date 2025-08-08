using Common.Application.DTOs;

namespace AuthService.Application.DTOs;

public record UserEntityDto : BaseEntityDto
{
    public RoleEntityDto? Role { get; set; }
    public StatusEntityDto? Status { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? LastLogin { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public bool Deleted { get; set; }
}