using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity 
{
    protected DbContext DbContext { get; }
    protected DbSet<TEntity> Entity { get; }

    protected Repository(AuthServiceDbContext dbContext)
    {
        DbContext = dbContext;
        Entity = dbContext.Set<TEntity>();
    }
    
    public virtual IEnumerable<TEntity> GetAll() => 
        Entity.ToList();
    
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => 
        await Entity.ToListAsync();
    
    public virtual TEntity? Get(int id) => 
        Entity.Find(id);
    
    public virtual async Task<TEntity?> GetAsync(int id) => 
        await Entity.FindAsync(id);
    
    public virtual TEntity? GetByCode(Guid code) => 
        Entity.FirstOrDefault(e => e.Code == code);
    
    public virtual async Task<TEntity?> GetByCodeAsync(Guid code) => 
        await Entity.FirstOrDefaultAsync(e => e.Code == code);

    public virtual void Add(TEntity entity)
    {
        Entity.Add(entity);
        DbContext.SaveChanges();
    }

    public virtual void Update(Guid code, TEntity entity)
    {
        if (!Entity.Any(e => e.Code.Equals(code))) 
            throw new NotFoundEntityException(code);
        
        Entity.Update(entity);
        DbContext.SaveChanges();
    }

    public virtual void Delete(Guid code)
    {
        var entity = Entity.FirstOrDefault(e => e.Code.Equals(code));
        if (entity == null) 
            throw new NotFoundEntityException(code);
        
        Entity.Remove(entity);
        DbContext.SaveChanges();
    }
}