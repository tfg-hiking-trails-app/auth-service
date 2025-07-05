using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs.Create;

public record CreateRoleEntityDto
{
    [Required]
    [MaxLength(50, ErrorMessage = "Role value must less than 50 characters")]
    public string? RoleValue { get; set; }
}