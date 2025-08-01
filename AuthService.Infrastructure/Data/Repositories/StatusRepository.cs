using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Common.Infrastructure.Data.Repositories;

namespace AuthService.Infrastructure.Data.Repositories;

public class StatusRepository : AbstractRepository<Status>, IStatusRepository
{
    public StatusRepository(AuthServiceDbContext dbContext) : base(dbContext)
    {
    }
}