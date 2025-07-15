using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;

namespace AuthService.Infrastructure.Data.Repositories;

public class RoleRepository : AbstractRepository<Role>, IRoleRepository
{
    public RoleRepository(AuthServiceDbContext dbContext) : base(dbContext)
    {
    }
    
    
}