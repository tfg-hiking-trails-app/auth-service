namespace AuthService.Application.DTOs.Create;

public record AccountCreationEntityDto
{
    public required Guid Code { get; set; }
    public required string Username { get; set; }
}
