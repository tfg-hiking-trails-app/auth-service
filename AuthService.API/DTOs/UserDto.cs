using AuthService.Application.DTOs;

namespace AuthService.API.DTOs;

public record UserDto(
    Guid Code,
    RoleDto? Role,
    StatusDto? Status,
    string Username,
    string Email,
    string? FirstName,
    string? LastName,
    DateTime? DateOfBirth,
    DateTime? LastLogin,
    string? ProfilePictureUrl
);
