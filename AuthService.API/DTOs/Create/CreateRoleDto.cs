using System.ComponentModel.DataAnnotations;
using Common.API.DTOs.Create;

namespace AuthService.API.DTOs.Create;

public record CreateRoleDto : CreateBaseDto
{
    [Required]
    [MaxLength(50, ErrorMessage = "Role value must less than 50 characters")]
    public string? RoleValue { get; set; }
}