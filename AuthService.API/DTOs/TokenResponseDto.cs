using System.Text.Json.Serialization;

namespace AuthService.API.DTOs;

public class TokenResponseDto
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
}