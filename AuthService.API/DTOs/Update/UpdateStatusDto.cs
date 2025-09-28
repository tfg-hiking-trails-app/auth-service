using System.ComponentModel.DataAnnotations;
using Common.API.DTOs.Update;

namespace AuthService.API.DTOs.Update;

public record UpdateStatusDto : UpdateBaseDto
{
    [MaxLength(50, ErrorMessage = "Status value must less than 50 characters")]
    public string? StatusValue { get; set; }
}