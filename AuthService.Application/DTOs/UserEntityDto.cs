namespace AuthService.Application.DTOs;

public record UserEntityDto
{
    public int Id { get; set; }
    public Guid Code { get; set; }
    public RoleEntityDto? Role { get; set; }
    public StatusEntityDto? Status { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? LastLogin { get; set; }
    public string? ProfilePictureUrl { get; set; }
}