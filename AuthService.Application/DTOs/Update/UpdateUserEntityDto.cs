namespace AuthService.Application.DTOs.Update;

public record UpdateUserEntityDto
{
    public Guid Code { get; init; }
    public Guid? RoleCode { get; set; }
    public Guid? StatusCode { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? LastLogin { get; set; }
    public string? ProfilePictureUrl { get; set; }
}