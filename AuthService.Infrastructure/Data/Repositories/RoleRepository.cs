using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Common.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data.Repositories;

public class RoleRepository : AbstractRepository<Role>, IRoleRepository
{
    public RoleRepository(AuthServiceDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Role?> GetByValueAsync(string roleValue)
    {
        if (string.IsNullOrWhiteSpace(roleValue))
            return null;

        return await Entity.FirstOrDefaultAsync(r => r.RoleValue.Equals(roleValue));
    }
}