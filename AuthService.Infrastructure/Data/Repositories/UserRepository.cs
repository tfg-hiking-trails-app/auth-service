using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AuthServiceDbContext dbContext) : base(dbContext)
    {
    }
    
    public override IEnumerable<User> GetAll() => 
        Entity
            .Include(u => u.Role)
            .Include(u => u.Status)
            .ToList();
    
    public override async Task<IEnumerable<User>> GetAllAsync() => 
        await Entity
            .Include(u => u.Role)
            .Include(u => u.Status)
            .ToListAsync();
    
    public override User? Get(int id) => 
        Entity
            .Include(u => u.Role)
            .Include(u => u.Status)
            .FirstOrDefault(e => e.Id == id);
    
    public override async Task<User?> GetAsync(int id) => 
        await Entity
            .Include(u => u.Role)
            .Include(u => u.Status)
            .FirstOrDefaultAsync(e => e.Id == id);
    
    public override User? GetByCode(Guid code) => 
        Entity
            .Include(u => u.Role)
            .Include(u => u.Status)
            .FirstOrDefault(e => e.Code == code);
    
    public override async Task<User?> GetByCodeAsync(Guid code) => 
        await Entity
            .Include(u => u.Role)
            .Include(u => u.Status)
            .FirstOrDefaultAsync(e => e.Code == code);
    
}