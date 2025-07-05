using AuthService.Application.DTOs;

namespace AuthService.API.DTOs;

public record UserDto
{
    public Guid Code { get; set; }

    public RoleEntityDto? Role { get; set; }

    public StatusEntityDto? Status { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public DateTime? LastLogin { get; set; }

    public string ProfilePictureUrl { get; set; } = string.Empty;
}