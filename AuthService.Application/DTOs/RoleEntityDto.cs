using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs;

public class RoleEntityDto
{
    [Required(ErrorMessage = "Code is required")]
    [Length(36, 36, ErrorMessage = "Code must be 36 characters")]
    public Guid Code { get; set; }
    
    [Required]
    [MaxLength(50, ErrorMessage = "Role value must less than 50 characters")]
    public string RoleValue { get; set; } = string.Empty;
}