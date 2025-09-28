using Common.Application.DTOs;

namespace AuthService.Application.DTOs;

public record RoleEntityDto : BaseEntityDto
{
    public string? RoleValue { get; set; }
}