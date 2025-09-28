using System.ComponentModel.DataAnnotations;
using Common.API.DTOs.Create;

namespace AuthService.API.DTOs.Create;

public record CreateStatusDto : CreateBaseDto
{
    [Required]
    [MaxLength(50, ErrorMessage = "Status value must less than 50 characters")]
    public string? StatusValue { get; set; }
}