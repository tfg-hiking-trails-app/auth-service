namespace AuthService.API.DTOs;

public abstract record BaseDto
{
    public Guid Code { get; set; }
}