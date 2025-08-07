using System.ComponentModel.DataAnnotations;
using Common.API.DataAnnotations;
using Common.API.DTOs.Create;

namespace AuthService.API.DTOs.Create;

public record CreateUserDto : CreateBaseDto
{
    [Required(ErrorMessage = "Role code is required")]
    [GuidValidator(ErrorMessage = "Role code must be a valid GUID")]
    public Guid RoleCode { get; set; }

    [Required(ErrorMessage = "Status code is required")]
    [GuidValidator(ErrorMessage = "Status code must be a valid GUID")]
    public Guid StatusCode { get; set; }
    
    [Required(ErrorMessage = "Username is required")]
    [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
    public required string Username { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public required string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [MaxLength(255, ErrorMessage = "Password cannot exceed 255 characters")]
    public required string Password { get; set; }
    
    [Required(ErrorMessage = "Confirm password is required")]
    [MaxLength(255, ErrorMessage = "Confirm password cannot exceed 255 characters")]
    public required string ConfirmPassword { get; set; }
    
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