namespace AuthService.API.DTOs;

public record StatusDto(
    Guid Code, 
    string StatusValue
);