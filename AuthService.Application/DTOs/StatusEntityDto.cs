using Common.Application.DTOs;

namespace AuthService.Application.DTOs;

public record StatusEntityDto : BaseEntityDto
{
    public string? StatusValue { get; set; }
}