using AuthService.Domain.Common;
using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    IEnumerable<TEntity> GetAll();
    
    Task<IPaged<TEntity>> GetPaged(FilterData filter, CancellationToken cancellationToken);
    
    Task<IEnumerable<TEntity>> GetAllAsync();
    
    TEntity? Get(int id);
    
    Task<TEntity?> GetAsync(int id);
    
    TEntity? GetByCode(Guid code);

    Task<TEntity?> GetByCodeAsync(Guid code);
    
    void Add(TEntity entity);
    
    void Update(Guid code, TEntity entity);
    
    void Delete(Guid code);
}