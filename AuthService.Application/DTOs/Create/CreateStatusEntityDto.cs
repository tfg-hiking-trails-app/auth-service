using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs.Create;

public record CreateStatusEntityDto
{
    [Required]
    [MaxLength(50, ErrorMessage = "Status value must less than 50 characters")]
    public string? StatusValue { get; set; }
}