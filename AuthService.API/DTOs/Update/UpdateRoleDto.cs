using System.ComponentModel.DataAnnotations;
using Common.API.DTOs.Update;

namespace AuthService.API.DTOs.Update;

public record UpdateRoleDto : UpdateBaseDto
{
    [MaxLength(50, ErrorMessage = "Role value must less than 50 characters")]
    public string? RoleValue { get; set; }
}