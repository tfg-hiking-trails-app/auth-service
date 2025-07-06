using AuthService.Application.Common.Extensions;
using AuthService.Domain.Common;
using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data.Repositories;

public class UserRepository : AbstractRepository<User>, IUserRepository
{
    public UserRepository(AuthServiceDbContext dbContext) : base(dbContext)
    {
    }

    public override IEnumerable<User> GetAll()
    {
        return Entity
            .Include(u => u.Role)
            .Include(u => u.Status)
            .ToList();
    }

    public override async Task<IEnumerable<User>> GetAllAsync()
    {
        return await Entity
            .Include(u => u.Role)
            .Include(u => u.Status)
            .ToListAsync();
    }

    public override async Task<IPaged<User>> GetPaged(
        FilterData filter, 
        CancellationToken cancellationToken)
    {
        return await Entity
            .Include(u => u.Role)
            .Include(u => u.Status)
            .ToPageAsync(filter, cancellationToken);
    }

    public override User? Get(int id)
    {
        return Entity
            .Include(u => u.Role)
            .Include(u => u.Status)
            .FirstOrDefault(e => e.Id == id);
    }

    public override async Task<User?> GetAsync(int id)
    {
        return await Entity
            .Include(u => u.Role)
            .Include(u => u.Status)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
    
    public override User? GetByCode(Guid code)
    {
        return Entity
            .Include(u => u.Role)
            .Include(u => u.Status)
            .FirstOrDefault(e => e.Code == code);
    }

    public override async Task<User?> GetByCodeAsync(Guid code)
    {
        return await Entity
            .Include(u => u.Role)
            .Include(u => u.Status)
            .FirstOrDefaultAsync(e => e.Code == code);
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return null;
        
        return await Entity
            .Include(u => u.Role)
            .Include(u => u.Status)
            .FirstOrDefaultAsync(e => e.Username.Equals(userName));
    }

    public override void Add(User entity)
    {
        if (Entity.Any(e => e.Username == entity.Username))
            throw new EntityAlreadyExistsException(nameof(User), "Username", entity.Username);
        
        if (Entity.Any(e => e.Email == entity.Email))
            throw new EntityAlreadyExistsException(nameof(User), "Email", entity.Email);
        
        base.Add(entity);
    }

}