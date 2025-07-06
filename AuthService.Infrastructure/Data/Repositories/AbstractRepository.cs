using AuthService.Application.Common.Extensions;
using AuthService.Domain.Common;
using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data.Repositories;

public abstract class AbstractRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity 
{
    protected DbContext DbContext { get; }
    protected DbSet<TEntity> Entity { get; }

    protected AbstractRepository(AuthServiceDbContext dbContext)
    {
        DbContext = dbContext;
        Entity = dbContext.Set<TEntity>();
    }
    
    public virtual IEnumerable<TEntity> GetAll() => 
        Entity.ToList();

    public virtual async Task<IPaged<TEntity>> GetPaged(FilterData filter, CancellationToken cancellationToken) =>
        await Entity.ToPageAsync(filter, cancellationToken);
    
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
            throw new NotFoundEntityException(nameof(TEntity), code);
        
        Entity.Update(entity);
        DbContext.SaveChanges();
    }

    public virtual void Delete(Guid code)
    {
        var entity = Entity.FirstOrDefault(e => e.Code.Equals(code));
        
        if (entity == null) 
            throw new NotFoundEntityException(nameof(TEntity), code);
        
        Entity.Remove(entity);
        DbContext.SaveChanges();
    }
}