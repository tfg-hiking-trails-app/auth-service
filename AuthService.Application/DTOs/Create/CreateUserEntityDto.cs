using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs.Create;

public class CreateUserEntityDto
{
    [Required(ErrorMessage = "Role is required")]
    public RoleEntityDto? Role { get; set; }
    
    [Required(ErrorMessage = "Status is required")]
    public StatusEntityDto? Status { get; set; }
    
    [Required(ErrorMessage = "Username is required")]
    [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email is required")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [MaxLength(255, ErrorMessage = "Password cannot exceed 255 characters")]
    public string Password { get; set; } = string.Empty;
    
    [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string FirstName { get; set; } = string.Empty;
    
    [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string LastName { get; set; } = string.Empty;
    
    public DateTime? DateOfBirth { get; set; }
    public DateTime? LastLogin { get; set; }
    
    [MaxLength(50, ErrorMessage = "Profile picture url cannot exceed 255 characters")]
    public string ProfilePictureUrl { get; set; } = string.Empty;
}