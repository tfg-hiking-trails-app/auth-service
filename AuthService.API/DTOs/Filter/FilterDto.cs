using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.Filter;

public record FilterDto
{
    public FilterDto(int pageNumber, int pageSize, string orderBy)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        OrderBy = orderBy;
    }
    
    [Range(1, int.MaxValue, ErrorMessage = "The page number must be greater than or equal to 1.")]
    public int PageNumber { get; set; } = 1;

    [Range(1, 50, ErrorMessage = "The page size must be between 1 and 50")]
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; } = "DESC";
}