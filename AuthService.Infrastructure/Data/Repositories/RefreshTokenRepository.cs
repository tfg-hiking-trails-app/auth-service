using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data.Repositories;

public class RefreshTokenRepository : AbstractRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AuthServiceDbContext dbContext) : base(dbContext)
    {
    }
    
    public override IEnumerable<RefreshToken> GetAll()
    {
        return Entity
            .Include(u => u.User)
            .ToList();
    }
    
    public override async Task<IEnumerable<RefreshToken>> GetAllAsync()
    {
        return await Entity
            .Include(u => u.User)
            .ToListAsync();
    }
    
    public override RefreshToken? Get(int id)
    {
        return Entity
            .Include(u => u.User)
            .FirstOrDefault(e => e.Id == id);
    }
    
    public override async Task<RefreshToken?> GetAsync(int id)
    {
        return await Entity
            .Include(u => u.User)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<RefreshToken?> FindByRefreshTokenAsync(string token)
    {
        return await Entity
            .Include(u => u.User)
            .FirstOrDefaultAsync(e => e.RefreshTokenValue == token);
    }

    public async Task<IEnumerable<RefreshToken>> GetAllValidRefreshTokensByUserAsync(Guid userCode)
    {
        return await Entity
            .Where(e => e.User!.Code.Equals(userCode) && e.Active && !e.Used)
            .ToListAsync();
    }
}