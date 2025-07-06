namespace AuthService.Domain.Common;

public record FilterData
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; } = "DESC";
}