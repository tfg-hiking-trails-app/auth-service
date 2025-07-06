namespace AuthService.Application.DTOs.Common;

public record FilterEntityDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; } = "DESC";
}