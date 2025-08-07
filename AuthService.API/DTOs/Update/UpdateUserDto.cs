using System.ComponentModel.DataAnnotations;
using Common.API.DataAnnotations;
using Common.API.DTOs.Update;

namespace AuthService.API.DTOs.Update;

public record UpdateUserDto : UpdateBaseDto
{
    [Required(ErrorMessage = "Code is required")]
    [GuidValidator(ErrorMessage = "Code must be a valid GUID")]
    public Guid Code { get; init; }
    
    [GuidValidator(ErrorMessage = "Role code must be a valid GUID")]
    public Guid RoleCode { get; set; }

    [GuidValidator(ErrorMessage = "Status code must be a valid GUID")]
    public Guid StatusCode { get; set; }
    
    [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
    public string? Username { get; set; }
    
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string? Email { get; set; }
    
    [MaxLength(255, ErrorMessage = "Password cannot exceed 255 characters")]
    public string? Password { get; set; }
    
    [MaxLength(255, ErrorMessage = "Confirm password cannot exceed 255 characters")]
    public string? ConfirmPassword { get; set; }
    
    [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string? FirstName { get; set; }
    
    [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string? LastName { get; set; }
    
    [DataType(DataType.DateTime)]
    public DateTime? DateOfBirth { get; set; }
    
    [DataType(DataType.DateTime)]
    public DateTime? LastLogin { get; set; }
    
    [MaxLength(255, ErrorMessage = "Profile picture url cannot exceed 255 characters")]
    public string? ProfilePictureUrl { get; set; }
}