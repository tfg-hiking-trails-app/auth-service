using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.Update;

public record UpdateStatusDto
{
    [MaxLength(50, ErrorMessage = "Status value must less than 50 characters")]
    public string? StatusValue { get; set; }
}