using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs.Update;

public record UpdateUserEntityDto
{
    [Required(ErrorMessage = "Code is required")]
    [Length(36, 36, ErrorMessage = "Code must be 36 characters")]
    public Guid Code { get; set; }
    
    [Required(ErrorMessage = "Role is required")]
    public RoleEntityDto? Role { get; set; }
    
    [Required(ErrorMessage = "Status is required")]
    public StatusEntityDto? Status { get; set; }
    
    [Required(ErrorMessage = "Username is required")]
    [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
    public string? Username { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [MaxLength(255, ErrorMessage = "Password cannot exceed 255 characters")]
    public string? Password { get; set; }
    
    [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string? FirstName { get; set; }
    
    [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string? LastName { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    public DateTime? LastLogin { get; set; }
    
    [MaxLength(50, ErrorMessage = "Profile picture url cannot exceed 255 characters")]
    public string? ProfilePictureUrl { get; set; }
}