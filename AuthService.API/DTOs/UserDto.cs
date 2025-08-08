using Common.API.DTOs;

namespace AuthService.API.DTOs;

public record UserDto : BaseDto
{
    public RoleDto? Role { get; set; }
    public StatusDto? Status { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? LastLogin { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public bool Deleted { get; set; }
}
