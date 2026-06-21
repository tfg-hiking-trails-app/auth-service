using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs;

public record RegisterDto
{
    [Required(ErrorMessage = "Username is required")]
    [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email format is not valid")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [MaxLength(255, ErrorMessage = "Password cannot exceed 255 characters")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Confirm password is required")]
    [MaxLength(255, ErrorMessage = "Confirm password cannot exceed 255 characters")]
    public required string ConfirmPassword { get; set; }
}
