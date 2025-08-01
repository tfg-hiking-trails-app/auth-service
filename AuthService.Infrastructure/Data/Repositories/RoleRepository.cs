using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Common.Infrastructure.Data.Repositories;

namespace AuthService.Infrastructure.Data.Repositories;

public class RoleRepository : AbstractRepository<Role>, IRoleRepository
{
    public RoleRepository(AuthServiceDbContext dbContext) : base(dbContext)
    {
    }
    
    
}