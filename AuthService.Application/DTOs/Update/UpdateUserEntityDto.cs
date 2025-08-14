namespace AuthService.Application.DTOs.Update;

public record UpdateUserEntityDto
{
    public Guid Code { get; init; }
    public Guid? RoleCode { get; set; }
    public Guid? StatusCode { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public bool? Deleted { get; set; }
}