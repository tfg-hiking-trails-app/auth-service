using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs;

public record AuthenticationDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Username must be at least 1 character")]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }
}