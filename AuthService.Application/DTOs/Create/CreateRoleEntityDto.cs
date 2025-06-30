using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs.Create;

public class CreateRoleEntityDto
{
    [Required]
    [MaxLength(50, ErrorMessage = "Role value must less than 50 characters")]
    public string RoleValue { get; set; } = string.Empty;
}