namespace AuthService.API.DTOs;

public record StatusDto : BaseDto
{
    public string? StatusValue { get; set; }
}