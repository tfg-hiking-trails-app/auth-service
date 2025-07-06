using AuthService.Application.Common.Pagination;
using AuthService.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static async Task<Page<T>> ToPageAsync<T>(
        this IQueryable<T> source,
        FilterData filter,
        CancellationToken cancellationToken
    )
    {
        if (filter.PageNumber < 1)
            throw new ArgumentException("PageNumber cannot be less than 1");
        if (filter.PageSize < 1)
            throw new ArgumentException("PageSize cannot be less than 1");

        int count = await source.CountAsync();
        List<T> items = await source
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new Page<T>(items, filter.PageNumber, filter.PageSize, count);
    }
}