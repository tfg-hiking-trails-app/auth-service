using Common.Application.DTOs.Create;

namespace AuthService.Application.DTOs.Create;

public record CreateStatusEntityDto : CreateBaseEntityDto
{
    public string? StatusValue { get; set; }
}