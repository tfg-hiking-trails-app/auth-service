using System.ComponentModel.DataAnnotations;
using AuthService.API.Utils;

namespace AuthService.API.DTOs.Filter;

public record FilterDto
{
    public FilterDto(int pageNumber, int pageSize, string? sortField, string sortDirection)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SortField = sortField;
        SortDirection = sortDirection;
    }
    
    [Range(1, int.MaxValue, ErrorMessage = "The page number must be greater than or equal to 1.")]
    public int PageNumber { get; set; } = Pagination.PAGE_NUMBER;

    [Range(1, 50, ErrorMessage = "The page size must be between 1 and 50")]
    public int PageSize { get; set; } = Pagination.PAGE_SIZE;

    public string? SortField { get; set; } = Pagination.SORT_FIELD;
    public string? SortDirection { get; set; } = Pagination.SORT_DIRECTION;
}