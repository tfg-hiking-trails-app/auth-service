namespace AuthService.Application.DTOs.Delete;

public record AccountDeletionEntityDto
{
    public required Guid Code { get; set; }
}
