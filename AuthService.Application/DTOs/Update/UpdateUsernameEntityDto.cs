namespace AuthService.Application.DTOs.Update;

public record UpdateUsernameEntityDto
{
    public required string OldUsername { get; set; }
    public required string NewUsername { get; set; }
}