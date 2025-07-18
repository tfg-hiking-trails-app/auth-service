﻿using System.Text.Json.Serialization;

namespace AuthService.API.DTOs;

public class TokenResponseDto
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }
}