using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs.Update;

public class UpdateStatusEntityDto
{
    [Required(ErrorMessage = "Code is required")]
    [Length(36, 36, ErrorMessage = "Code must be 36 characters")]
    public Guid Code { get; set; }
    
    [Required]
    [MaxLength(50, ErrorMessage = "Status value must less than 50 characters")]
    public string StatusValue { get; set; } = string.Empty;
}