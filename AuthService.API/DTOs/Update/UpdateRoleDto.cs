using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.Update;

public record UpdateRoleDto
{
    [MaxLength(50, ErrorMessage = "Role value must less than 50 characters")]
    public string? RoleValue { get; set; }
}