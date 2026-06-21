using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Common.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data.Repositories;

public class StatusRepository : AbstractRepository<Status>, IStatusRepository
{
    public StatusRepository(AuthServiceDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Status?> GetByValueAsync(string statusValue)
    {
        if (string.IsNullOrWhiteSpace(statusValue))
            return null;

        return await Entity.FirstOrDefaultAsync(s => s.StatusValue.Equals(statusValue));
    }
}