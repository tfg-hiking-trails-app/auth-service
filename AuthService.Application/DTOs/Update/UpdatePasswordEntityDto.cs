namespace AuthService.Application.DTOs.Update;

public record UpdatePasswordEntityDto
{
    public required string Password { get; set; }
    public required string NewPassword { get; set; }
    public required string ConfirmNewPassword { get; set; }
}