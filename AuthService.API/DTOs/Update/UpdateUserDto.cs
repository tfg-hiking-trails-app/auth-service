using System.ComponentModel.DataAnnotations;
using Common.API.DataAnnotations;
using Common.API.DTOs.Update;

namespace AuthService.API.DTOs.Update;

public record UpdateUserDto : UpdateBaseDto
{
    [GuidValidator(ErrorMessage = "Role code must be a valid GUID")]
    public Guid RoleCode { get; set; }

    [GuidValidator(ErrorMessage = "Status code must be a valid GUID")]
    public Guid StatusCode { get; set; }
    
    [MaxLength(255, ErrorMessage = "Password cannot exceed 255 characters")]
    public string? Password { get; set; }
    
    [MaxLength(255, ErrorMessage = "Confirm password cannot exceed 255 characters")]
    public string? ConfirmPassword { get; set; }
    
    public bool? Deleted { get; set; }
}