using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.Update;

public record UpdatePasswordDto
{
    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
    [Required(ErrorMessage = "NewPassword is required")]
    public required string NewPassword { get; set; }
    [Required(ErrorMessage = "ConfirmNewPassword is required")]
    public required string ConfirmNewPassword { get; set; }
}