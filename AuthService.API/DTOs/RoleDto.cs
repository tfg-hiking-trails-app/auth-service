using Common.API.DTOs;

namespace AuthService.API.DTOs;

public record RoleDto : BaseDto
{
    public string? RoleValue { get; set; }
}