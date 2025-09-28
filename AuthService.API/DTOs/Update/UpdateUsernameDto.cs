using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.Update;

public record UpdateUsernameDto
{
    [Required(ErrorMessage = "Username is required")]
    public required string Username { get; set; }
}