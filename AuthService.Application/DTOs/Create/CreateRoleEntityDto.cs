using Common.Application.DTOs.Create;

namespace AuthService.Application.DTOs.Create;

public record CreateRoleEntityDto : CreateBaseEntityDto
{
    public string? RoleValue { get; set; }
}